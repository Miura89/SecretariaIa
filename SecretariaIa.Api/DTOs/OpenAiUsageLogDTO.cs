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
		public int DurationMs { get; set; }
		public bool Success { get; set; }
		public string PlanName { get; set; } = string.Empty;
		public int OperationType { get; set; }
		public string InputType { get; set; } = string.Empty;
		public string? PromptVersion { get; set; } = string.Empty;
		public int? OutputCharacters { get; set; }
		public int? InputCharacters { get; set; }
		public string? IdentityName { get; set; } = string.Empty;
	}
	public class OpenAiUsageLogSumCostDTO
	{
		public decimal SumCostUsd { get; set; }
	}
}
