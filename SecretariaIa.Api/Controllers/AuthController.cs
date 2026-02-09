using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SecretariaIa.Api.Queries.IdentityUserQueries;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Common.Exceptions;
using SecretariaIa.Domain.Commands.IdentityUserCommands;

namespace SecretariaIa.Api.Controllers
{
	[ApiController]
	[Route("v1/auth")]
	public class AuthController : CustomControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<AuthController> _logger;
		private readonly TokenConfigDTO _tokenConfig;

		public AuthController(IMediator mediator, ILogger<AuthController> logger, IOptions<TokenConfigDTO> tokenConfig)
		{
			_mediator = mediator;
			_logger = logger;
			_tokenConfig = tokenConfig.Value;
		}

		[HttpPost("sign")]
		public async Task<IActionResult> SignIn([FromBody] SignIdentityUserCommand command, CancellationToken cancellationToken)
		{
			var response = await _mediator.Send(command, cancellationToken);
			if (!response.Success)
				throw new CommandResponseException(response);

			var token = await TokenUtil.CreateTokenAsync(async () =>
			{
				var user = await _mediator.Send(new GetIdentityUserByEmail(command.Email), cancellationToken);
				return new UserTokenDTO(user.Id.ToString(), user.Email, user.Name, user.Role);
			}, _tokenConfig);

			_logger.LogInformation("User [{email}] authenticated successfully", command.Email);
			return Result(token, 200);
		}

	}
}
