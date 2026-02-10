using Microsoft.Extensions.Configuration;
using SecretariaIa.Common.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class TwilioWhatsAppSender : ITwilioWhatsAppSender
{
	private readonly string _from;

	public TwilioWhatsAppSender(IConfiguration config)
	{
		var sid = config["Twilio:AccountSid"];
		var token = config["Twilio:AuthToken"];
		_from = config["Twilio:WhatsAppFrom"];

		if (string.IsNullOrWhiteSpace(sid) ||
			string.IsNullOrWhiteSpace(token) ||
			string.IsNullOrWhiteSpace(_from))
			throw new InvalidOperationException("Configuração do Twilio inválida.");

		TwilioClient.Init(sid, token);
	}

	public async Task<string> SendAsync(string to, string text)
	{
		if (string.IsNullOrWhiteSpace(to))
			throw new ArgumentException("Destino inválido", nameof(to));

		if (string.IsNullOrWhiteSpace(text))
			throw new ArgumentException("Mensagem vazia", nameof(text));

		try
		{
			var message = await MessageResource.CreateAsync(
				from: new PhoneNumber(_from),
				to: new PhoneNumber(to),
				body: text
			);

			return message.Sid;
		}
		catch (Exception ex)
		{
			throw new ApplicationException("Erro ao enviar WhatsApp via Twilio", ex);
		}
	}
}
