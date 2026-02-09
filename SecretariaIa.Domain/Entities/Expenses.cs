using SecretariaIa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Domain.Entities
{
	public class Expenses : Entity<Guid>
	{
		public Expenses()
		{
			
		}
		public Expenses(IdentityUser identityUser, Guid identityUserId, decimal? amount, string? description, DateTime? occureedAt, Category? category, Currency? currency)
		{
			SetIdentityUser(identityUser, identityUserId);
			SetAmount(amount);
			SetDescription(description);
			SetOccureedAt(occureedAt);
			SetCategory(category);
			SetCurrency(currency);

			Validate();
		}

		public IdentityUser IdentityUser { get; private set; }
		public Guid IdentityUserId { get; private set; }
		public decimal? Amount { get; private set; }
		public string? Description { get; private set; } = string.Empty;
		public DateTime? OccureedAt { get; private set; }
		public Category? Category { get; private set; }
		public Currency? Currency { get; private set; }
		
		public Expenses SetIdentityUser(IdentityUser identityUser,  Guid identityUserId)
		{
			IdentityUser = identityUser;
			IdentityUserId = identityUserId;
			return this;
		}
		public Expenses SetAmount(decimal? amount)
		{
			Amount = amount;
			return this;
		}
		public Expenses SetDescription(string? description)
		{
			Description = description;
			return this;
		}
		public Expenses SetOccureedAt(DateTime? occureedAt)
		{
			OccureedAt = occureedAt;
			return this;
		}
		public Expenses SetCategory(Category? category)
		{
			Category = category;
			return this;
		}
		public Expenses SetCurrency(Currency? currency)
		{
			Currency = currency;
			return this;
		}
		public override bool Validate()
		{
			Clear();
			return IsValid;
		}
	}
}
