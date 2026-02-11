using Microsoft.Extensions.Configuration;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Common.Interfaces;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Enums;
using SecretariaIa.Domain.Interfaces;
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
		private readonly IOpenAiUsageLogRepository _repository;

		public OpenAiService(HttpClient http, IConfiguration config, IOpenAiUsageLogRepository repository)
		{
			_http = http;
			_apiKey = config["OpenAI:ApiKey"]!;
			_repository = repository;
		}

		public async Task<AiParsedResult> ParseMessage(string message, string examplesJson, Plan? plan)
		{
			_http.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", _apiKey);

			var schema = """
					Extraia um JSON válido, sem texto extra.

					intent: 1=create_expense, 2=get_summary
					currency: sempre 1 (BRL)
					category: 1=Alimentacao,2=Transporte,3=Moradia,4=Saude,5=Lazer,6=Contas,7=Compras,8=Educacao,9=Outros,0=Unknown

					create_expense:
					{
					 intent:number,
					 amount:number|null,
					 currency:1,
					 category:number,
					 description:string|null,
					 occurredAt:"today"|"yesterday"|null,
					 needs_clarification:boolean,
					 confidence:number,
					 missing_fields:string[]|null
					}

					get_summary:
					{
					 intent:number,
					 needs_clarification:boolean,
					 confidence:number,
					 missing_fields:string[]|null
					}

					Regras:
					- valor no texto => amount obrigatório
					- sem data => occurredAt="today"
					- se faltar amount ou description => needs_clarification=true
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
				temperature = 0.0
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

			var result = JsonSerializer.Deserialize<AiParsedResult>(content);

			var usageElement = doc.RootElement.GetProperty("usage");
			int promptTokens = usageElement.GetProperty("prompt_tokens").GetInt32();
			int completionTokens = usageElement.GetProperty("completion_tokens").GetInt32();

			decimal cost = (promptTokens / 1000m) * 0.003m + (completionTokens / 1000m) * 0.006m;

			OpenAiUsageLog log = new()
			{
				RequestId = doc.RootElement.GetProperty("id").GetString()!,
				Model = doc.RootElement.GetProperty("model").GetString()!,
				PromptTokens = promptTokens,
				CompletionTokens = completionTokens,
				TotalTokens = usageElement.GetProperty("total_tokens").GetInt32(),
				CostUsd = cost,
				Timestamp = DateTime.UtcNow
			};

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

		public async Task<AiParsedResult> ParseAudio(
			Stream audio,
			string examplesJson,
			Plan? plan)
		{
			var text = await TranscribeAudio(audio);

			return await ParseMessage(text, examplesJson, plan);
		}


		public async Task<string> TranscribeAudio(Stream audioStream)
		{
			_http.DefaultRequestHeaders.Authorization =
				   new AuthenticationHeaderValue("Bearer", _apiKey);

			using var content = new MultipartFormDataContent();

			content.Add(new StreamContent(audioStream), "file", "audio.wav");
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
	}
}
