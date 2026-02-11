using MediatR;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Domain.Models;

namespace SecretariaIa.Api.Queries.IdentityUserQueries
{
	public class VerifySubscriptionByIdentityNumber : IRequest<Plan?>
	{
		public string Phone { get; set; }

		public VerifySubscriptionByIdentityNumber(string phone)
		{
			Phone = phone;
		}
	}
	public class VerifySubscriptionByIdentityNumberHandler : IRequestHandler<VerifySubscriptionByIdentityNumber, Plan?>
	{
		private readonly ISubscriptionRepository _repository;
		private readonly IPlanRepository _planRepository;

		public VerifySubscriptionByIdentityNumberHandler(ISubscriptionRepository repository, IPlanRepository planRepository)
		{
			_repository = repository;
			_planRepository = planRepository;
		}

		public async Task<Plan?> Handle(VerifySubscriptionByIdentityNumber request, CancellationToken cancellationToken)
		{
			var subscription = await _repository.VerifySubscription(request.Phone);
			if (subscription == null)
				return null;

			return await _planRepository.FindAsync(x => x.Id == subscription.PlanId);
		}
	}
}
