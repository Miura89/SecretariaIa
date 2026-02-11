using Microsoft.EntityFrameworkCore;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Enums;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Infrasctructure.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Infrasctructure.Data.Repositories
{
	public class SubscriptionRepository : Repository<Guid, Subscription>, ISubscriptionRepository
	{
		public SubscriptionRepository(ApplicationContext context) : base(context)
		{
		}

		public async Task<Subscription?> VerifySubscription(string phone)
		{
			phone = phone.Replace("whatsapp:", "");

			var identity = await _context.Set<IdentityUser>()
				.FirstOrDefaultAsync(x => x.Phone == phone && x.Type == TypeUser.CUSTOMER);

			if (identity is null)
				return null;

			var now = DateTime.UtcNow;

			var subscription = await _context.Set<Subscription>()
				.Where(s =>
					s.IdentityUserId == identity.Id &&
					(s.Status == SubscriptionStatus.Active ||
					 s.Status == SubscriptionStatus.Trailing) &&
					(s.EndDate == null || s.EndDate > now) &&
					(s.TrialEndDate == null || s.TrialEndDate > now)
				)
				.OrderByDescending(s => s.StartDate)
				.FirstOrDefaultAsync();

			return subscription;
		}

	}
}
