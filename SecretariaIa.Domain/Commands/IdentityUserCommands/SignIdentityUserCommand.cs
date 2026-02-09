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
	public class SignIdentityUserCommand : IRequest<CommandResultResponse<Guid>> 
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public TypeUser Type { get; set; }
	}
	public class SignIdentityUserCommandHandler : IRequestHandler<SignIdentityUserCommand, CommandResultResponse<Guid>>
	{
		private readonly IIdentityUserRepository _repository;
		private readonly IPasswordHash _hash;

		public SignIdentityUserCommandHandler(IIdentityUserRepository repository, IPasswordHash hash)
		{
			_repository = repository;
			_hash = hash;
		}

		public async Task<CommandResultResponse<Guid>> Handle(SignIdentityUserCommand request, CancellationToken cancellationToken)
		{
			CommandResultResponse<Guid> response = new();

			var user = await _repository.FindAsync(x => x.Email == request.Email && x.Type == request.Type);
			if (user == null)
				return response.AddNotifications("Email ou senha incorreta");

			if (!_hash.Verify(request.Password, user.Password))
				return response.AddNotifications("Email ou senha incorreta");

			response.SetResult(user.Id);
			return response;
		}
	}
}
