using MediatR;
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
	public class UpdateIdentityUserCommand : IRequest<CommandResultResponse<Guid>>
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public Roles Role { get; set; }
		public Country Country { get; set; }
		public Guid UpdatedBy { get; set; }
	}
	public class UpdateIdentityUserCommandHandler : IRequestHandler<UpdateIdentityUserCommand, CommandResultResponse<Guid>>
	{
		private readonly IIdentityUserRepository _repository;

		public UpdateIdentityUserCommandHandler(IIdentityUserRepository repository)
		{
			_repository = repository;
		}

		public async Task<CommandResultResponse<Guid>> Handle(UpdateIdentityUserCommand request, CancellationToken cancellationToken)
		{
			CommandResultResponse<Guid> response = new();

			var user = await _repository.FindAsync(x => x.Id == request.Id);
			if (user == null)
				return response.AddNotifications("Usuario não encontrado.");

			var phone = await _repository.FindAsync(x => x.Phone == request.Phone && x.Id != request.Id);
			if (phone != null)
				return response.AddNotifications("Telefone já cadastrado.");

			var email = await _repository.FindAsync(x=>x.Email.ToLower() == request.Email && x.Id != request.Id);
			if (email != null)
				return response.AddNotifications("Email já cadastrado.");

			user
				.SetName(request.Name)
				.SetEmail(request.Email)
				.SetPhone(request.Phone)
				.SetRole(request.Role)
				.SetCountry(request.Country);
			await _repository.UpdateAsync(user, request.UpdatedBy);
			await _repository.UnitOfWork.Commit(cancellationToken);

			response.SetResult(user.Id);
			return response;
		}
	}
}
