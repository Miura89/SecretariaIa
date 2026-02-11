using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretariaIa.Api.Queries.OpenAiUsageLogQueries;

namespace SecretariaIa.Api.Controllers
{
	[ApiController]
	[Route("v1/openai-usage-logs")]
	[Authorize]
	public class OpenAiUsageLogsController : CustomControllerBase
	{
		private readonly IMediator _mediator;

		public OpenAiUsageLogsController(IMediator mediator)
		{
			_mediator = mediator;
		}
		[HttpGet("search")]
		public async Task<IActionResult> Search([FromQuery] SearchOpenAiUsageLogQuerie query, CancellationToken cancellationToken)
		{
			CheckOperatorRequirement();
			var response = await _mediator.Send(query, cancellationToken);
			return Ok(response);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken)
		{
			CheckOperatorRequirement();
			var response = await _mediator.Send(new GetOpenAiUsageLogByIdQuery(id), cancellationToken);

			return Ok(response);
		}
		[HttpGet("sum")]
		public async Task<IActionResult> SumCost(CancellationToken cancellationToken)
		{
			CheckOperatorRequirement();
			var response = await _mediator.Send(new GetOpenAiUsageLogSumCostQuery(), cancellationToken);
			return Ok(response);
		}

	}
}
