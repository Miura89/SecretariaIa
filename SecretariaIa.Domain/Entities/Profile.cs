using SecretariaIa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Domain.Entities
{
	public class Profile : Entity<Guid>
	{
		public Profile()
		{
			
		}
		public Profile(Currency? currency, string? timeZone, Language? language, decimal? monthlyBudget, IdentityUser identityUser, Guid identityUserId)
		{
			SetCurrency(currency);
			SetTimeZone(timeZone);
			SetLanguage(language);
			SetMonthlyBudget(monthlyBudget);
			SetIdentityUser(identityUser, identityUserId);

			Validate();
		}
		public IdentityUser IdentityUser { get; set; }
		public Guid IdentityUserId { get; set; }
		public Currency? Currency { get; private set; }
		public string? TimeZone { get; private set; } = string.Empty;
		public Language? Language { get; private set; }
		public decimal? MonthlyBudget { get; private set; }

		public Profile SetIdentityUser(IdentityUser identityUser, Guid identityUserId)
		{
			IdentityUser = identityUser;
			IdentityUserId = identityUserId;
			return this;
		}
		public Profile SetCurrency(Currency? currency)
		{
			Currency = currency;
			return this;
		}
		public Profile SetTimeZone(string? timeZone)
		{
			TimeZone = timeZone;
			return this;
		}
		public Profile SetLanguage(Language? language)
		{
			Language = language;
			return this;
		}
		public Profile SetMonthlyBudget(decimal? monthlyBudget)
		{
			MonthlyBudget = monthlyBudget;
			return this;
		}
		public override bool Validate()
		{
			Clear();
			return IsValid;
		}
	}
}
