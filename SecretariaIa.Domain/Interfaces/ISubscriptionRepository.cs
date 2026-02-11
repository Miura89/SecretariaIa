using SecretariaIa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Interfaces
{
	public interface ISubscriptionRepository : IRepositoryRead<Guid, Subscription>, IRepositoryWrite<Guid, Subscription>
	{
		Task<Subscription?> VerifySubscription(string phone);
	}
}
