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

namespace SecretariaIa.Domain.Commands.PlanCommands
{
	public class CreatePlanCommand : IRequest<CommandResultResponse<Guid>>
	{
		public string PlanName { get; set; } = string.Empty;
		public string PlanDescription { get; set; } = string.Empty;
		public decimal PriceUSD { get; set; }
		public int MaxMessagesPerMonth { get; set; }
		public decimal MaxOpenAiUsdPerMonth { get; set; }
		public bool IsActive { get; set; }
		public PlanLimitBehavior LimitBehavior { get; set; }
		public OpenAiModel DefaultMode { get; set; }
		public Guid? CreatedBy { get; set; }
	}
	public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, CommandResultResponse<Guid>>
	{
		private readonly IPlanRepository _repository;

		public CreatePlanCommandHandler(IPlanRepository repository)
		{
			_repository = repository;
		}

		public async Task<CommandResultResponse<Guid>> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
		{
			CommandResultResponse<Guid> response = new();

			Plan plan = new(request.PlanName, request.PlanDescription, request.PriceUSD, request.MaxMessagesPerMonth, request.MaxOpenAiUsdPerMonth, request.IsActive, request.LimitBehavior, request.DefaultMode);

			await _repository.CreateAsync(plan, request.CreatedBy);
			await _repository.UnitOfWork.Commit(cancellationToken);

			response.SetResult(plan.Id);

			return response;
		}
	}
}
