using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.DTOs
{
	public class OpenAiUsageDTO
	{
		public string RequestId { get; set; } = default!;
		public string Model { get; set; } = default!;
		public int PromptTokens { get; set; }
		public int CompletionTokens { get; set; }
		public int TotalTokens { get; set; }
		public decimal CostUsd { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	}

}
