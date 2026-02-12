using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Domain.Models;

namespace SecretariaIa.Api.Queries.IdentityUserQueries
{
	public class VerifySubscriptionByIdentityNumber : IRequest<ValidAcessDTO?>
	{
		public string Phone { get; set; }

		public VerifySubscriptionByIdentityNumber(string phone)
		{
			Phone = phone;
		}
	}
	public class VerifySubscriptionByIdentityNumberHandler : IRequestHandler<VerifySubscriptionByIdentityNumber, ValidAcessDTO?>
	{
		private readonly ISubscriptionRepository _repository;
		private readonly IPlanRepository _planRepository;
		private readonly IIdentityUserRepository _identityUserRepository;

		public VerifySubscriptionByIdentityNumberHandler(ISubscriptionRepository repository, IPlanRepository planRepository, IIdentityUserRepository identityUserRepository)
		{
			_repository = repository;
			_planRepository = planRepository;
			_identityUserRepository = identityUserRepository;
		}

		public async Task<ValidAcessDTO?> Handle(VerifySubscriptionByIdentityNumber request, CancellationToken cancellationToken)
		{
			var subscription = await _repository.VerifySubscription(request.Phone);
			if (subscription == null)
				return null;

			var plan = await _planRepository.FindAsync(x => x.Id == subscription.PlanId);
			if (plan == null)
				return null;

			var identityUser = await _identityUserRepository.FindAsync(x => x.Id == subscription.IdentityUserId);
			if (identityUser == null)
				return null;

			ValidAcessDTO dto = new ValidAcessDTO
			{
				Subscription = subscription,
				Plan = plan,
				IdentityUser = identityUser
			};
			return dto;
		}
	}
}
