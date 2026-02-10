using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretariaIa.Api.Queries.PlanQueries;
using SecretariaIa.Common.Exceptions;
using SecretariaIa.Domain.Commands.PlanCommands;

namespace SecretariaIa.Api.Controllers
{
	[ApiController]
	[Route("v1/plan")]
	[Authorize]
	public class PlanController : CustomControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<PlanController> _logger;

		public PlanController(IMediator mediator, ILogger<PlanController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreatePlanCommand command, CancellationToken cancellationToken)
		{
			CheckOperatorRequirement();
			command.CreatedBy = GetAuthenticatedUserId();

			var response = await _mediator.Send(command, cancellationToken);
			if (!response.Success)
				throw new CommandResponseException(response);

			return Result(response, 201);
		}
		[HttpGet("search")]
		public async Task<IActionResult> Search([FromQuery] SearchPlanQuery query, CancellationToken cancellationToken)
		{
			CheckOperatorRequirement();

			var response = await _mediator.Send(query, cancellationToken);
			return Ok(response);
		}
	}
}
