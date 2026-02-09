using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.RequestDTO
{
	public class AiParsedResult
	{
		[JsonPropertyName("intent")]
		public int Intent { get; set; }

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

		[JsonPropertyName("needs_clarification")]
		public bool NeedsClarification { get; set; }

		[JsonPropertyName("confidence")]
		public double Confidence { get; set; }

		[JsonPropertyName("missing_fields")]
		public string[]? MissingFields { get; set; }
	}
}
