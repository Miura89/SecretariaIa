using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Enums
{
	public enum SubscriptionStatus
	{
		Active = 1,
		Canceled = 2,
		Expired = 3,
		Suspended = 4,
		PastDue = 5,
		Trailing = 6
	}
}
