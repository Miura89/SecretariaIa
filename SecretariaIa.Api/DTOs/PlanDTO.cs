using SecretariaIa.Domain.Enums;

namespace SecretariaIa.Api.DTOs
{
	public class PlanDTO
	{
		public Guid Id { get; set; }
		public string PlanName { get; set; } = string.Empty;
		public string PlanDescription { get; set; } = string.Empty;
		public decimal PriceUSD { get; set; }
		public decimal MaxMessagesPerMonth { get; set; }
		public decimal MaxOpenAiUsdPerMonth { get; set; }
		public bool IsActive { get; set; }
		public PlanLimitBehavior LimitBehaviore { get; set; }
		public OpenAiModel DefaultMode { get; set; }
	}
}
