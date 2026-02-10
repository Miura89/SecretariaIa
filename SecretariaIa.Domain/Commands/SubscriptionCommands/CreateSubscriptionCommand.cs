using MediatR;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Enums;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Commands.SubscriptionCommands
{
	public class GeneratedTrialCommand : IRequest<CommandResultResponse<Guid>>
	{
		public Guid IdentityUserId { get; set; }
		public Guid PlanId { get; set; }
		public Guid? CreatedBy { get; set; }
	}
	public class GeneratedTrialCommandHandler : IRequestHandler<GeneratedTrialCommand, CommandResultResponse<Guid>>
	{
		private readonly ISubscriptionRepository _repository;
		private readonly IPlanRepository _planRepository;
		private readonly IIdentityUserRepository _identityUserRepository;
		public GeneratedTrialCommandHandler(ISubscriptionRepository repository, IPlanRepository planRepository, IIdentityUserRepository identityUserRepository)
		{
			_repository = repository;
			_planRepository = planRepository;
			_identityUserRepository = identityUserRepository;
		}

		public async Task<CommandResultResponse<Guid>> Handle(GeneratedTrialCommand request, CancellationToken cancellationToken)
		{
			CommandResultResponse<Guid> response = new();

			var plan = await _planRepository.FindAsync(x => x.Id == request.PlanId);
			if (plan == null)
				return response.AddNotifications("Plano não encontrado.");

			var active = await _repository.FindAsync(x => x.IdentityUserId == request.IdentityUserId && x.Status == SubscriptionStatus.Active);

			if (active != null)
				return response.AddNotifications("Usuário já possui uma assinatura ativa.");

			var identityUser = await _identityUserRepository.FindAsync(x => x.Id == request.IdentityUserId);
			if (identityUser == null)
				return response.AddNotifications("Usuário não encontrado.");

			Subscription subscription = new(identityUser.Id, identityUser, plan.Id, plan, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), DateTime.UtcNow, DateTime.UtcNow.AddDays(7), SubscriptionStatus.Trailing, 0);

			await _repository.CreateAsync(subscription, request.CreatedBy);
			await _repository.UnitOfWork.Commit(cancellationToken);

			response.SetResult(subscription.Id);
			return response;
		}
	}
}
