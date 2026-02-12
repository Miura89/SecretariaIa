using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.RequestDTO
{
	public class AiParsedResult
	{
		[JsonPropertyName("domain")]
		public string Domain { get; set; }
		[JsonPropertyName("intent")]
		public int Intent { get; set; }

		[JsonPropertyName("needs_clarification")]
		public bool NeedsClarification { get; set; }

		[JsonPropertyName("confidence")]
		public double Confidence { get; set; }

		[JsonPropertyName("missing_fields")]
		public string[]? MissingFields { get; set; }
		[JsonPropertyName("payload")]
		public JsonElement Payload { get; set; }

		public T? GetPayload<T>()
		{
			if (Payload.ValueKind == JsonValueKind.Undefined || Payload.ValueKind == JsonValueKind.Null)
				return default;

			return Payload.Deserialize<T>();
		}
	}
	public class CreateExpenseResult
	{
		[JsonPropertyName("amount")]
		public decimal? Amount { get; set; }

		[JsonPropertyName("currency")]
		public int Currency { get; set; }

		[JsonPropertyName("category")]
		public int Category { get; set; }

		[JsonPropertyName("description")]
		public string? Description { get; set; }
		[JsonPropertyName("occurredAt")]
		public string? OccurredAt { get; set; }
	}
	public class CreateAppointmentResult
	{
		[JsonPropertyName("title")]
		public string Title { get; set; } = default!;
		[JsonPropertyName("scheduledAt")]
		public DateTime ScheduledAt { get; set; }
		[JsonPropertyName("remindBeforeMinutes")]
		public int? RemindBeforeMinutes { get; set; }
	}
}
