using Microsoft.Extensions.Configuration;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Common.Interfaces;
using SecretariaIa.Domain.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SecretariaIa.Infrasctructure.Data.Services
{
	public class OpenAiService : IOpenAiService
	{
		private readonly HttpClient _http;
		private readonly string _apiKey;

		public OpenAiService(HttpClient http, IConfiguration config)
		{
			_http = http;
			_apiKey = config["OpenAI:ApiKey"]!;
		}

		public async Task<AiParsedResult> ParseMessage(string message, string examplesJson)
		{
			_http.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", _apiKey);

			var schema = """
Você é um extrator de gastos.
Retorne SOMENTE um JSON válido (sem texto extra, sem markdown).

Formato EXATO:
{
  "intent": number,
  "amount": number|null,
  "currency": number,
  "category": number,
  "description": string|null,
  "occurredAt": "today"|"yesterday"|null,
  "needs_clarification": boolean,
  "confidence": number,
  "missing_fields": string[]|null
}

Enums:
intent: 1=create_expense, 0=unknown
currency: 1=BRL
category: 1=Alimentacao, 2=Transporte, 3=Moradia, 4=Saude, 5=Lazer, 6=Contas, 7=Compras, 8=Educacao, 9=Outros, 0=Unknown

Regras:
- Se houver valor no texto, amount NÃO pode ser null.
- Sempre currency=1.
- Se não houver data, occurredAt="today".
- Se faltar amount => needs_clarification=true e missing_fields inclui "amount".
- Se faltar description => needs_clarification=true e missing_fields inclui "description".
""";

			var payload = new
			{
				model = "gpt-4.1-mini",
				response_format = new { type = "json_object" },
				messages = new object[]
				{
			new { role = "system", content = schema },
			new { role = "system", content = "Exemplos (imite o expected):\n" + examplesJson },
			new { role = "user", content = message }
				},
				temperature = 0.1
			};

			var response = await _http.PostAsJsonAsync(
				"https://api.openai.com/v1/chat/completions",
				payload
			);
			response.EnsureSuccessStatusCode();

			var raw = await response.Content.ReadAsStringAsync();

			using var doc = JsonDocument.Parse(raw);
			var content = doc.RootElement
				.GetProperty("choices")[0]
				.GetProperty("message")
				.GetProperty("content")
				.GetString();

			if (string.IsNullOrWhiteSpace(content))
				throw new InvalidOperationException("OpenAI retornou content vazio.");

			return JsonSerializer.Deserialize<AiParsedResult>(
				content,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			)!;
		}

	}
}
