namespace SecretariaIa.Api.DTOs
{
	public class OpenAiUsageLogDTO
	{
		public Guid Id { get; set; }
		public string RequestId { get; set; } = default!;
		public string Model { get; set; } = default!;
		public int PromptTokens { get; set; }
		public int CompletionTokens { get; set; }
		public int TotalTokens { get; set; }
		public decimal CostUsd { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	}
	public class OpenAiUsageLogSumCostDTO
	{
		public decimal SumCostUsd { get; set; }
	}
}
