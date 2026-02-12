
using Microsoft.Extensions.Configuration;
using SecretariaIa.Common.Interfaces;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Enums;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Domain.RequestDTO;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SecretariaIa.Infrasctructure.Data.Services
{
	public class OpenAiService : IOpenAiService
	{
		private readonly HttpClient _http;
		private readonly string _apiKey;
		private readonly IOpenAiUsageLogRepository _repository;

		public OpenAiService(HttpClient http, IConfiguration config, IOpenAiUsageLogRepository repository)
		{
			_http = http;
			_apiKey = config["OpenAI:ApiKey"]!;
			_repository = repository;
		}

		public async Task<AiParsedResult> ParseMessage(string message, string examplesJson, Plan plan, Subscription subscription, IdentityUser identity)
		{
			_http.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", _apiKey);
			var now = DateTime.Now;

			var schema = $"""
						Data atual: {DateTime.Now:yyyy-MM-dd}.
						Considere essa data como referência absoluta para interpretar datas relativas como:
						- hoje
						- amanhã
						- depois de amanhã
						- próxima segunda
						- daqui a X dias
						Extraia um JSON válido, sem texto extra.

						INTENTS:
						1 = create_expense
						2 = get_summary
						3 = create_appointment

						CURRENCY:
						currency: sempre 1 (BRL)

						CATEGORY (para despesas):
						1=Alimentacao
						2=Transporte
						3=Moradia
						4=Saude
						5=Lazer
						6=Contas
						7=Compras
						8=Educacao
						9=Outros
						0=Unknown

						====================================
						ESTRUTURA OBRIGATÓRIA
						====================================

						"domain": "finance" | "appointment",
						  "intent": number,
						  "needs_clarification": boolean,
						  "confidence": number,
						  "missing_fields": string[] | null,
						  "payload": object | null
						

						====================================
						INTENT 1 — create_expense
						====================================

						
						  "domain": "finance",
						  "intent": 1,
						  "needs_clarification": boolean,
						  "confidence": number,
						  "missing_fields": string[] | null,
						  "payload": 
							"amount": number | null,
							"currency": 1,
							"category": number,
							"description": string | null,
							"occurredAt": "today" | "yesterday" | null
						 

						Regras:
						- Se houver valor explícito → amount obrigatório
						- Se não houver data → occurredAt = "today"
						- Se faltar amount ou description → needs_clarification = true
						- Se categoria não identificável → category = 0

						====================================
						INTENT 2 — get_summary
						====================================

						
						  "domain": "finance",
						  "intent": 2,
						  "needs_clarification": boolean,
						  "confidence": number,
						  "missing_fields": string[] | null,
						  "payload": null
						

						Regras:
						- Deve ser usado quando o usuário pedir resumo, extrato ou listar gastos.

						====================================
						INTENT 3 — create_appointment
						====================================

						
						  "domain": "appointment",
						  "intent": 3,
						  "needs_clarification": boolean,
						  "confidence": number,
						  "missing_fields": string[] | null,
						  "payload": 
							"title": string | null,
							"scheduledAt": string | null,
							"remindBeforeMinutes": number | null
						  
						

						Regras:
						- scheduledAt deve estar no formato ISO 8601 (YYYY-MM-DDTHH:MM:SS)
						- Converter expressões como "amanhã às 15:30" para data completa
						- Se não houver horário definido → needs_clarification = true
						- Se não houver título claro → inferir do texto
						- Se houver "me avisa X minutos antes" → preencher remindBeforeMinutes
						- Se não houver lembrete mencionado → remindBeforeMinutes = null

						====================================

						REGRAS GERAIS:
						- Retorne apenas JSON válido
						- Nunca retorne explicações
						- confidence deve variar entre 0 e 1
						- Se faltar informação obrigatória → needs_clarification = true
						""";



			var model = ResolveModel(plan?.DefaultMode ?? OpenAiModel.Gpt4oMini);

			var messages = new List<object>
			{
				new { role = "system", content = schema },
				new { role = "user", content = message }
			};


			var payload = new
			{
				model,
				response_format = new { type = "json_object" },
				messages,
				temperature = 0.1
			};
			var stopwatch = Stopwatch.StartNew();

			var response = await _http.PostAsJsonAsync(
				"https://api.openai.com/v1/chat/completions",
				payload
			);
			response.EnsureSuccessStatusCode();
			stopwatch.Stop();
			var durationMs = stopwatch.ElapsedMilliseconds;

			var raw = await response.Content.ReadAsStringAsync();

			using var doc = JsonDocument.Parse(raw);
			var content = doc.RootElement
				.GetProperty("choices")[0]
				.GetProperty("message")
				.GetProperty("content")
				.GetString();

			var result = JsonSerializer.Deserialize<AiParsedResult>(content);

			var usageElement = doc.RootElement.GetProperty("usage");
			int promptTokens = usageElement.GetProperty("prompt_tokens").GetInt32();
			int completionTokens = usageElement.GetProperty("completion_tokens").GetInt32();

			decimal cost = (promptTokens / 1000m) * 0.003m + (completionTokens / 1000m) * 0.006m;

			OpenAiUsageLog log = new(doc.RootElement.GetProperty("id").GetString()!, doc.RootElement.GetProperty("model").GetString()!, promptTokens, completionTokens, usageElement.GetProperty("total_tokens").GetInt32(), cost, DateTime.UtcNow, (int)durationMs, response.IsSuccessStatusCode, "", result.Intent.ToString(), "text", identity.Id, identity, subscription, subscription.Id, plan, plan.Id, message.Length, content.Length, "v1");

			await _repository.CreateAsync(log, Guid.Empty);
			await _repository.UnitOfWork.Commit();

			if (string.IsNullOrWhiteSpace(content))
				throw new InvalidOperationException("OpenAI retornou content vazio.");

			return JsonSerializer.Deserialize<AiParsedResult>(
				content,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			)!;
		}
		private static string ResolveModel(OpenAiModel? model)
		{
			return model switch
			{
				OpenAiModel.Gpt4oMini => "gpt-4o-mini",
				OpenAiModel.Gpt41 => "gpt-4.1",
				_ => "gpt-4o-mini"
			};
		}

		public async Task<string> TranscribeAudio(Stream audioStream, string mimeType)
		{
			_http.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", _apiKey);

			using var content = new MultipartFormDataContent();

			using var ms = new MemoryStream();
			await audioStream.CopyToAsync(ms);
			ms.Position = 0;

			var fileContent = new StreamContent(ms);
			fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);

			var fileName = mimeType.EndsWith("ogg") ? "audio.ogg" : "audio.wav";
			content.Add(fileContent, "file", fileName);

			content.Add(new StringContent("gpt-4o-mini-transcribe"), "model");
			content.Add(new StringContent("pt"), "language");

			var response = await _http.PostAsync(
				"https://api.openai.com/v1/audio/transcriptions",
				content
			);

			response.EnsureSuccessStatusCode();

			var raw = await response.Content.ReadAsStringAsync();

			using var doc = JsonDocument.Parse(raw);
			return doc.RootElement.GetProperty("text").GetString()!;
		}

		public async Task<AiParsedResult> ParseAudio(Stream audio, string examplesJson, Plan plan, Subscription subscription, IdentityUser identity, string mimeType = "audio/ogg")
		{
			var text = await TranscribeAudio(audio, mimeType);

			return await ParseMessage(text, examplesJson, plan, subscription, identity);
		}
	}
}
