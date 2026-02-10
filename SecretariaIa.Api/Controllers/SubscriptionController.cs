using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretariaIa.Api.Queries.SubscriptionQueries;
using SecretariaIa.Domain.Commands.SubscriptionCommands;

namespace SecretariaIa.Api.Controllers
{
	[ApiController]
	[Route("v1/subscription")]
	[Authorize]
	public class SubscriptionController : CustomControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<SubscriptionController> _logger;

		public SubscriptionController(IMediator mediator, ILogger<SubscriptionController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}
		[HttpPost("generate-trial")]
		public async Task<IActionResult> GenerateSubscription([FromBody] GeneratedTrialCommand command, CancellationToken cancellationToken)
		{
			CheckMasterRequirement();
			command.CreatedBy = GetAuthenticatedUserId();
			var result = await _mediator.Send(command, cancellationToken);

			if (!result.Success)
				throw new Common.Exceptions.CommandResponseException(result);

			return Result(result, 201);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult> GetSubscriptionsByIdentityUsers([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			CheckOperatorRequirement();

			var response = await _mediator.Send(new GetSubscriptionByIdentityUserIdQuery(id), cancellationToken);
			return Ok(response);
		}

	}
}
