using Microsoft.AspNetCore.Mvc;
using SecretariaIa.Common.Interfaces;

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

		public TwilioWebhookController(IOpenAiService openAiService, IWebHostEnvironment env, ILogger<TwilioWebhookController> logger, ITwilioWhatsAppSender sender)
		{
			_openAiService = openAiService;
			_env = env;
			_logger = logger;
			_sender = sender;
		}
		[HttpPost("whatsapp")]
		public async Task<IActionResult> Receive([FromForm] TwilioInboundDto inbound)
		{
			_logger.LogInformation("Inbound: From={From} Body={Body} sid={Sid}", [inbound.From, inbound.Body, inbound.MessageSid]);

			var userPhone = inbound.From.Replace("whatsapp:", "").Trim();

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
			if (parsed.NeedsClarification)
			{
				reply = parsed.MissingFields?.Contains("amount") == true
					? "Qual foi o valor? (ex: 37,90)"
					: "Consegue detalhar um pouco mais?";
			}
			else
			{
				reply = $"Anotei ✅ R$ {parsed.Amount:0.00} (cat {parsed.Category}).";
			}

			await _sender.SendAsync(userPhone, reply);
			return Ok();
		}

		public record TwilioInboundDto(
			[FromForm(Name = "From")] string From,
			[FromForm(Name = "To")] string To,
			[FromForm(Name = "Body")] string Body,
			[FromForm(Name = "MessageSid")] string MessageSid
			);
	}
}
