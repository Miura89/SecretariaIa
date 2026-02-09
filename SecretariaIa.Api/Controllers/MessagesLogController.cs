using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretariaIa.Api.Queries.MessagesLogMonitoringQueries;

namespace SecretariaIa.Api.Controllers
{
	[ApiController]
	[Authorize]
	[Route("v1/logs")]
	public class MessagesLogController : CustomControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<MessagesLogController> _logger;

		public MessagesLogController(IMediator mediator, ILogger<MessagesLogController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}
		[HttpGet("search")]
		public async Task<IActionResult> Search([FromQuery] SearchMessagesLog query, CancellationToken cancellationToken)
		{
			CheckOperatorRequirement();
			var response = await _mediator.Send(query, cancellationToken);
			return Ok(response);
		}
		[HttpGet("by/id/{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			CheckOperatorRequirement();
			var response = await _mediator.Send(new GetMessagesLogById(id), cancellationToken);
			return Ok(response);
		}
	}
}
