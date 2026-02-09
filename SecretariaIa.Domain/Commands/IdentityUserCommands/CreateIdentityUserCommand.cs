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

namespace SecretariaIa.Domain.Commands.IdentityUserCommands
{
	public class CreateIdentityUserCommand : IRequest<CommandResultResponse<Guid>>
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Phone { get; set; }
		public Roles Role { get; set; }
		public TypeUser TypeUser { get; set; }
		public string FormatedPhone { get; set; }
		public Country Country { get; set; }
		public Currency? Currency { get; set; }
		public decimal? MonthlyIncome { get; set; }
		public Language? Language { get; set; }
		public string? TimeZone { get; set; }
		public Guid? CreateBy { get; set; }
	}
	public class CreateIdentityUserCommandHandler : IRequestHandler<CreateIdentityUserCommand, CommandResultResponse<Guid>>
	{
		private readonly IIdentityUserRepository _repository;
		private readonly IProfileRepository _profileRepository;
		private readonly IPasswordHash _hash;
		public CreateIdentityUserCommandHandler(IIdentityUserRepository repository, IProfileRepository profileRepository, IPasswordHash hash)
		{
			_repository = repository;
			_profileRepository = profileRepository;
			_hash = hash;
		}

		public async Task<CommandResultResponse<Guid>> Handle(CreateIdentityUserCommand request, CancellationToken cancellationToken)
		{
			CommandResultResponse<Guid> response = new();

			var phone = await _repository.FindAsync(x => x.Phone == request.Phone);
			if (phone != null)
				return response.AddNotifications("Telefone já cadastrado");

			var email = await _repository.FindAsync(x => x.Email == request.Email);
			if (email != null)
				return response.AddNotifications("E-mail já cadastrado");

			var hash = _hash.GenerateHash(request.Password);

			if(request.TypeUser == TypeUser.CUSTOMER)
			{
				request.Role = Roles.Customer;
			}

			IdentityUser entity = new(request.Name, request.Email, hash!, request.Phone, request.TypeUser, request.Role, request.FormatedPhone, request.Country);
			await _repository.CreateAsync(entity, request.CreateBy);

			if(entity.Type == TypeUser.CUSTOMER)
			{
				Profile profile = new(request.Currency, request.TimeZone, request.Language, request.MonthlyIncome, entity, entity.Id);
				await _profileRepository.CreateAsync(profile, request.CreateBy);
			}

			await _repository.UnitOfWork.Commit(cancellationToken);

			response.SetResult(entity.Id);
			return response;
		}
	}
}
