using MediatR;
using SecretariaIa.Domain.Enums;

namespace SecretariaIa.Api.DTOs
{
	public class SubscriptionDTO
	{
		public string Phone { get; set; } = string.Empty;
		public string IdentityName { get; set; } = string.Empty;
		public string PlanName { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime TrialStartDate { get; set; }
		public DateTime TrialEndDate { get; set; }
		public SubscriptionStatus Status { get; set; }
		public decimal AmountPaid { get; set; }
	}
}
