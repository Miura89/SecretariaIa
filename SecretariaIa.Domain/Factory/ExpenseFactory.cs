using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Enums;
using SecretariaIa.Domain.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Factory
{
	public static class ExpenseFactory
	{
		public static Expenses Factory(CreateExpenseResult request, IdentityUser identityUser, Profile profile)
		{
			Category category;
			switch (request.Category)
			{
				case 1:
					category = Category.Food;
					break;
				case 2:
					category = Category.Transport;
					break;
				case 3:
					category = Category.Housing;
					break;
				case 4:
					category = Category.Health;
					break;
				case 5:
					category = Category.Leisure;
					break;
				case 6:
					category = Category.Bills;
					break;
				case 7:
					category = Category.Shopping;
					break;
				case 8:
					category = Category.Education;
					break;
				default:
					category = Category.Other;
					break;
			}

			DateTime.TryParse(request.OccurredAt, out DateTime occurredAt);
			if(occurredAt == DateTime.MinValue)
			{
				occurredAt = DateTime.UtcNow;
			}
			Expenses expense = new(identityUser, identityUser.Id, request.Amount, request.Description, occurredAt, category, profile.Currency);
			return expense;
		}
	}
}
