using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretariaIa.Api.Queries.IdentityUserQueries;
using SecretariaIa.Common.Exceptions;
using SecretariaIa.Domain.Commands.IdentityUserCommands;

namespace SecretariaIa.Api.Controllers
{
	[ApiController]
	[Route("v1/identity")]
	[Authorize]
	public class IdentityUserController : CustomControllerBase
	{
		private readonly ILogger<IdentityUserController> _logger;
		private readonly IMediator _mediator;

		public IdentityUserController(ILogger<IdentityUserController> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateIdentityUserCommand command, CancellationToken cancellationToken)
		{
			CheckMasterRequirement();

			var response = await _mediator.Send(command, cancellationToken);
			if (!response.Success)
				throw new CommandResponseException(response);

			return Result(response, 201);
		}
		[HttpPatch]
		public async Task<IActionResult> Update(UpdateIdentityUserCommand command, CancellationToken cancellationToken)
		{
			CheckMasterRequirement();

			var response = await _mediator.Send(command, cancellationToken);
			if (!response.Success)
				throw new CommandResponseException(response);

			return Result(response, 200);
		}

		[HttpGet("search")]
		public async Task<IActionResult> Search([FromQuery] SearchIdentityUser query, CancellationToken cancellationToken)
		{
			CheckOperatorRequirement();
			var response = await _mediator.Send(query, cancellationToken);
			return Ok(response);
		}
		[HttpGet("all")]
		public async Task<IActionResult> All([FromQuery] AllIdentityUser query, CancellationToken cancellationToken)
		{
			CheckOperatorRequirement();
			var response = await _mediator.Send(query, cancellationToken);
			return Ok(response);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			CheckOperatorRequirement();
			var response = await _mediator.Send(new GetIdentityUserById(id), cancellationToken);
			return Ok(response);
		}
	}
}
