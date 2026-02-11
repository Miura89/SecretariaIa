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
using System.Text;
using Twilio.TwiML.Voice;

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
		private readonly string _twilioSid;
		private readonly string _twilioAuthToken;
		public TwilioWebhookController(IOpenAiService openAiService, IWebHostEnvironment env, ILogger<TwilioWebhookController> logger, ITwilioWhatsAppSender sender, IMediator mediator, ITrainingSamplesProvider provider, IConfiguration config)
		{
			_openAiService = openAiService;
			_env = env;
			_logger = logger;
			_sender = sender;
			_mediator = mediator;
			_provider = provider;
			_twilioSid = config["Twilio:AccountSid"]!;
			_twilioAuthToken = config["Twilio:AuthToken"]!;
		}
		[HttpPost("sms")]
		public async Task<IActionResult> Receive([FromForm] TwilioInboundDto inbound, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Inbound: From={From} Body={Body} sid={Sid}", [inbound.From, inbound.Body, inbound.MessageSid]);
			string reply;
			var userPhone = inbound.From;

			var plan = await _mediator.Send(new VerifySubscriptionByIdentityNumber(userPhone), cancellationToken);
			if (plan is null)
			{
				reply = "⚠️ Seu plano atual expirou!\r\nPara continuar usando todos os recursos, clique no link abaixo e renove seu plano:\r\n Renovar Plano\r\n\r\nAssim você não perde o histórico e continua aproveitando todos os benefícios.";
				await _sender.SendAsync(userPhone, reply);
				return Unauthorized();
			}

			var examplesJson = await _provider
				.GetCreateExpenseSamplesAsync(cancellationToken);

			if (!string.IsNullOrEmpty(inbound.MediaUrl0))
			{
				_logger.LogInformation("Processando áudio...");
				using var httpClient = new HttpClient();
				var byteArray = Encoding.ASCII.GetBytes($"{_twilioSid}:{_twilioAuthToken}");
				httpClient.DefaultRequestHeaders.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

				using var audioStream = await httpClient.GetStreamAsync(inbound.MediaUrl0);
				using var ms = new MemoryStream();
				await audioStream.CopyToAsync(ms);
				ms.Position = 0;

				var parsed = await _openAiService.ParseAudio(ms, examplesJson, plan, inbound.MediaContentType0!);
				return await HandleParsed(parsed, inbound.From, cancellationToken);
			}
			else
			{
				_logger.LogInformation("Processando texto...");
				var parsed = await _openAiService.ParseMessage(inbound.Body, examplesJson, plan);
				return await HandleParsed(parsed, inbound.From, cancellationToken);
			}
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
