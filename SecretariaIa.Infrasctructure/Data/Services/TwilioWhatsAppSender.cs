using Microsoft.Extensions.Configuration;
using SecretariaIa.Common.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class TwilioWhatsAppSender : ITwilioWhatsAppSender
{
	private readonly IConfiguration _config;

	public TwilioWhatsAppSender(IConfiguration config) => _config = config;

	public Task<string> SendAsync(string toE164, string text)
	{
		var sid = _config["Twilio:AccountSid"]!;
		var token = _config["Twilio:AuthToken"]!;
		var from = _config["Twilio:WhatsAppFrom"]!;

		TwilioClient.Init(sid, token);

		var msg = MessageResource.Create(
			from: new PhoneNumber(from),
			to: new PhoneNumber("whatsapp:" + toE164),
			body: text
		);

		return Task.FromResult(msg.Sid);
	}
}
