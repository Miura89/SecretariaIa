using MediatR;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Enums;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Domain.Models;
using SecretariaIa.Domain.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Commands.ExpensesCommands
{
	public class CreateExpensesCommand : IRequest<CommandResultResponse<Guid>>
	{
		public CreateExpensesCommand(AiParsedResult result, string phone)
		{
			Result = result;
			Phone = phone;
		}

		public AiParsedResult Result { get; init; }
		public string Phone { get; set; }
		public Guid? CreateBy { get; set; }
	}
	public class CreateExpensesCommandHandler : IRequestHandler<CreateExpensesCommand, CommandResultResponse<Guid>>
	{
		private readonly IExpensesRepository _repository;
		private readonly IIdentityUserRepository _identityRepository;
		private readonly IProfileRepository _profileRepository;
		private readonly IMessagesLogsRepository _logRepository;
		public CreateExpensesCommandHandler(IExpensesRepository repository, IIdentityUserRepository identityRepository, IProfileRepository profileRepository, IMessagesLogsRepository logRepository)
		{
			_repository = repository;
			_identityRepository = identityRepository;
			_profileRepository = profileRepository;
			_logRepository = logRepository;
		}

		public async Task<CommandResultResponse<Guid>> Handle(CreateExpensesCommand request, CancellationToken cancellationToken)
		{
			CommandResultResponse<Guid> response = new();
		
			var identityUser = await _identityRepository.FindAsync(x => x.Phone == request.Phone);
			if (identityUser == null)
				return response.AddNotifications("Usuario não encontrado");

			request.CreateBy = identityUser.Id;

			var profile = await _profileRepository.FindAsync(x=>x.IdentityUserId == identityUser.Id);
			if(profile == null)
				return response.AddNotifications("Perfil do usuario não encontrado");

			var factory = Factory.ExpenseFactory.Factory(request.Result, identityUser, profile);

			MessageLog messageLog = new("", identityUser.Phone, DateTime.UtcNow, CommandsMessage.CreateExpense, "", request.Result.Confidence, StatusMessage.SENDING, request.Result.NeedsClarification, identityUser, identityUser.Id);
			
			await _logRepository.CreateAsync(messageLog, identityUser.Id);
			await _repository.CreateAsync(factory, request.CreateBy);
			await _repository.UnitOfWork.Commit(cancellationToken);

			response.SetResult(factory.Id);

			return response;
		}
	}
}
