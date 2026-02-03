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

			var datasetPath = Path.Combine(_env.ContentRootPath, "SecretariaIa.Api", "Ai", "TrainingSamples", "training_command_create_expense.json");

			if (!System.IO.File.Exists(datasetPath))
			{
				_logger.LogError("Dataset não encontrado: {path}", datasetPath);
				return Ok(); // evita crash e evita retry louco do Twilio
			}

			var examplesJson = System.IO.File.ReadAllText(datasetPath);
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
