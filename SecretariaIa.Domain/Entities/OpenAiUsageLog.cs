using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Entities
{
	public class OpenAiUsageLog : Entity<Guid>
	{
		public OpenAiUsageLog()
		{
			
		}
		public OpenAiUsageLog(string requestId, string model, int promptTokens, int completionTokens, int totalTokens, decimal costUsd, DateTime timestamp)
		{
			RequestId = requestId;
			Model = model;
			PromptTokens = promptTokens;
			CompletionTokens = completionTokens;
			TotalTokens = totalTokens;
			CostUsd = costUsd;
			Timestamp = timestamp;
		}

		public string RequestId { get; set; } = default!;
		public string Model { get; set; } = default!;
		public int PromptTokens { get; set; }
		public int CompletionTokens { get; set; }
		public int TotalTokens { get; set; }
		public decimal CostUsd { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	}
}
