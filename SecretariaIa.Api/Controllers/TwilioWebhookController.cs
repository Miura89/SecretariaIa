using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Common.Interfaces;
using SecretariaIa.Domain.Commands.ExpensesCommands;

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

		public TwilioWebhookController(IOpenAiService openAiService, IWebHostEnvironment env, ILogger<TwilioWebhookController> logger, ITwilioWhatsAppSender sender, IMediator mediator)
		{
			_openAiService = openAiService;
			_env = env;
			_logger = logger;
			_sender = sender;
			_mediator = mediator;
		}
		[HttpPost("sms")]
		public async Task<IActionResult> Receive([FromForm] TwilioInboundDto inbound, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Inbound: From={From} Body={Body} sid={Sid}", [inbound.From, inbound.Body, inbound.MessageSid]);

			var userPhone = inbound.From;

			var examplesJson = """
				{
				  "version":"1.0",
				  "samples":[
					{"message":"Gastei 40 no posto","expected":{"intent":1,"amount":40,"currency":1,"category":2,"description":"posto","occurredAt":"today","needs_clarification":false,"confidence":0.9,"missing_fields":null}}
				  ]
				}
				""";

			var parsed = await _openAiService.ParseMessage(inbound.Body, examplesJson);

			string reply;
			const double CONFIDENCE_THRESHOLD = 0.80;

			if (parsed.NeedsClarification || parsed.Confidence < CONFIDENCE_THRESHOLD)
			{
				reply = parsed.MissingFields?.Contains("amount") == true
					? "Qual foi o valor? (ex: 37,90)"
					: "Consegue detalhar um pouco mais?";
			}
			else
			{
				reply = $"Anotei ✅ R$ {parsed.Amount:0.00} (cat {parsed.Category}).";
			}
			var response = await _mediator.Send(new CreateExpensesCommand(parsed, userPhone), cancellationToken);
			
			if (response.Success)
				await _sender.SendAsync(userPhone, reply);

			_logger.LogInformation("Amount: {}, category: {}", parsed.Amount, parsed.Category);
			return Ok();
		}
	}
}
