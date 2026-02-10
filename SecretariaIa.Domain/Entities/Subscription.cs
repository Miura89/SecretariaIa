using SecretariaIa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Entities
{
	public class Subscription : Entity<Guid>
	{
		public Subscription()
		{
			
		}
		public Subscription(Guid identityUserId, IdentityUser identityUser, Guid planId, Plan plan, DateTime startDate, DateTime? endDate, DateTime? trialStartDate, DateTime? trialEndDate, SubscriptionStatus status, decimal amountPaid)
		{
			IdentityUserId = identityUserId;
			IdentityUser = identityUser;
			PlanId = planId;
			Plan = plan;
			StartDate = startDate;
			EndDate = endDate;
			TrialStartDate = trialStartDate;
			TrialEndDate = trialEndDate;
			Status = status;
			AmountPaid = amountPaid;
		}
		public Guid IdentityUserId { get; set; }
		public IdentityUser IdentityUser { get; set; } = null!;
		public Guid PlanId { get; set; }
		public Plan Plan { get; set; } = null!;
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? TrialStartDate { get; set; }
		public DateTime? TrialEndDate { get; set; }
		public SubscriptionStatus Status { get; set; }
		public decimal AmountPaid { get; set; }
		public bool IsInTrial =>
			TrialEndDate.HasValue &&
			TrialEndDate > DateTime.UtcNow;
		public bool IsActive => EndDate == null || EndDate > DateTime.UtcNow;
	}
}
