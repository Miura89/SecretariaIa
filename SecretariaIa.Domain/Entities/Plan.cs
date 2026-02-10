using SecretariaIa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Entities
{
	public class Plan : Entity<Guid>
	{
		public Plan()
		{
			
		}
		public Plan(string planName, string planDescription, decimal priceUSD, int maxMessagesPerMonth, decimal maxOpenAiUsdPerMonth, bool isActive, PlanLimitBehavior limitBehaviore, OpenAiModel defaultMode)
		{
			SetPlanName(planName);
			SetPlanDescription(planDescription);
			SetPriceUSD(priceUSD);
			SetMaxMessagesPerMonth(maxMessagesPerMonth);
			SetMaxOpenAiUsdPerMonth(maxOpenAiUsdPerMonth);
			SetIsActive(isActive);
			SetLimitBehavior(limitBehaviore);
			SetDefaultMode(defaultMode);
		}

		public string PlanName { get; set; } = string.Empty;
		public string PlanDescription { get; set; } = string.Empty;
		public decimal PriceUSD { get; set; }
		public int MaxMessagesPerMonth { get; set; }
		public decimal MaxOpenAiUsdPerMonth { get; set; }
		public bool IsActive { get; set; }
		public PlanLimitBehavior LimitBehaviore { get; set; }
		public OpenAiModel DefaultMode { get; set; }

		public Plan SetPlanName(string planName)
		{
			PlanName = planName;
			return this;
		}
		public Plan SetPlanDescription(string planDescription)
		{
			PlanDescription = planDescription;
			return this;
		}
		public Plan SetPriceUSD(decimal priceUSD)
		{
			PriceUSD = priceUSD;
			return this;
		}
		public Plan SetMaxMessagesPerMonth(int maxMessagesPerMonth)
		{
			MaxMessagesPerMonth = maxMessagesPerMonth;
			return this;
		}
		public Plan SetMaxOpenAiUsdPerMonth(decimal maxOpenAiUsdPerMonth)
		{
			MaxOpenAiUsdPerMonth = maxOpenAiUsdPerMonth;
			return this;
		}
		public Plan SetIsActive(bool isActive)
		{
			IsActive = isActive;
			return this;
		}
		public Plan SetLimitBehavior(PlanLimitBehavior limitBehavior)
		{
			LimitBehaviore = limitBehavior;
			return this;
		}
		public Plan SetDefaultMode(OpenAiModel defaultMode)
		{
			DefaultMode = defaultMode;
			return this;
		}
	}
}
