using MediatR;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Domain.Models;

namespace SecretariaIa.Api.Queries.IdentityUserQueries
{
	public class VerifySubscriptionByIdentityNumber : IRequest<bool>
	{
		public string Phone { get; set; }

		public VerifySubscriptionByIdentityNumber(string phone)
		{
			Phone = phone;
		}
	}
	public class VerifySubscriptionByIdentityNumberHandler : IRequestHandler<VerifySubscriptionByIdentityNumber, bool>
	{
		private readonly ISubscriptionRepository _repository;

		public VerifySubscriptionByIdentityNumberHandler(ISubscriptionRepository repository)
		{
			_repository = repository;
		}

		public async Task<bool> Handle(VerifySubscriptionByIdentityNumber request, CancellationToken cancellationToken)
		{
			var subscription = await _repository.VerifySubscription(request.Phone);
			if (subscription == null)
				return false;
			return true;
		}
	}
}
