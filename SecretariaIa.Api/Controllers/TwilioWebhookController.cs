using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecretariaIa.Api.AI.TrainingSamples;
using SecretariaIa.Api.Queries.ExpensesUserQueries;
using SecretariaIa.Api.Queries.IdentityUserQueries;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Common.Exceptions;
using SecretariaIa.Common.Interfaces;
using SecretariaIa.Common.Util;
using SecretariaIa.Domain.Commands.ExpensesCommands;
using SecretariaIa.Domain.RequestDTO;

namespace SecretariaIa.Api.Controllers
{
	[Route("webhook/twilio")]
	[ApiController]
	public class TwilioWebhookController : ControllerBase
	{
		private readonly IOpenAiService _openAiService;
		private readonly IWebHostEnvironment _env;
		private readonly ITwilioWhatsAppSender _sender;
		private readonly ILogger<TwilioWebhookController> _logger;
		private readonly IMediator _mediator;
		private readonly ITrainingSamplesProvider _provider;

		public TwilioWebhookController(IOpenAiService openAiService, IWebHostEnvironment env, ILogger<TwilioWebhookController> logger, ITwilioWhatsAppSender sender, IMediator mediator, ITrainingSamplesProvider provider)
		{
			_openAiService = openAiService;
			_env = env;
			_logger = logger;
			_sender = sender;
			_mediator = mediator;
			_provider = provider;
		}
		[HttpPost("sms")]
		public async Task<IActionResult> Receive([FromForm] TwilioInboundDto inbound, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Inbound: From={From} Body={Body} sid={Sid}", [inbound.From, inbound.Body, inbound.MessageSid]);

			var userPhone = inbound.From;

			var plan = await _mediator.Send(new VerifySubscriptionByIdentityNumber(userPhone), cancellationToken);
			if (plan is null)
				throw new UnauthorizedAccessException();

			var examplesJson = await _provider
				.GetCreateExpenseSamplesAsync(cancellationToken);


			var parsed = await _openAiService.ParseMessage(inbound.Body, examplesJson, plan);

			_logger.LogInformation("Amount: {}, category: {}", parsed.Amount, parsed.Category);
			return await HandleParsed(parsed, userPhone, cancellationToken);
		}
		[HttpPost("voice")]
		public async Task<IActionResult> ReceiveVoice([FromForm] TwilioInboundDto inbound, CancellationToken cancellationToken)
		{
			var userPhone = inbound.From;

			var plan = await _mediator.Send(new VerifySubscriptionByIdentityNumber(userPhone), cancellationToken);
			if (plan is null)
				throw new UnauthorizedAccessException();

			var audioUrl = inbound.MediaUrl0;
			using var httpClient = new HttpClient();
			using var audioStream = await httpClient.GetStreamAsync(audioUrl);

			var examplesJson = await _provider.GetCreateExpenseSamplesAsync(cancellationToken);

			var parsed = await _openAiService.ParseAudio(audioStream, examplesJson, plan);

			return await HandleParsed(parsed, userPhone, cancellationToken);
		}


		private async Task<IActionResult> HandleParsed(AiParsedResult parsed, string userPhone, CancellationToken ct)
		{
			string reply;
			const double CONFIDENCE_THRESHOLD = 0.80;

			if (parsed.Intent == 1)
			{
				if (parsed.Confidence < CONFIDENCE_THRESHOLD || parsed.NeedsClarification)
				{
					reply = parsed.MissingFields?.Contains("amount") == true
							? "Qual foi o valor? (ex: 37,90)"
							: "Consegue detalhar um pouco mais?";
					await _sender.SendAsync(userPhone, reply);
				}
				else
				{
					reply = $"Anotei ✅ R$ {parsed.Amount:0.00} (cat {CategoryFormatter.Format(parsed.Category)}).";
					var response = await _mediator.Send(new CreateExpensesCommand(parsed, userPhone), ct);
					if (response.Success)
						await _sender.SendAsync(userPhone, reply);
				}
			}

			if (parsed.Intent == 2)
			{
				var response = await _mediator.Send(new GetExpensesByDayQuery(userPhone), ct);

				if (response.Any())
				{
					reply = "Seus gastos:\n" +
						string.Join("\n", response.Select(e => $"- R$ {e.Value:0.00} ({CategoryFormatter.Format(e.Category)}) - {e.Description} - {DateFormatter.FormatDateTimeHuman(e.Date)}"));
					await _sender.SendAsync(userPhone, reply);
				}
				else
				{
					reply = "Você não tem gastos registrados nesse período.";
					await _sender.SendAsync(userPhone, reply);
				}
			}

			_logger.LogInformation("Amount: {}, category: {}", parsed.Amount, parsed.Category);
			return Ok();
		}



	}
}
