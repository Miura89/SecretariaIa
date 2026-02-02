namespace SecretariaIa.Api.Models
{
	public class InboundMessage
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Provider { get; set; } = "twilio";
		public string Channel { get; set; } = "whatsapp";

		public string From { get; set; } = default!;
		public string To { get; set; } = default!;
		public string Body { get; set; } = default!;

		public string? MessageSid { get; set; }
		public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;
	}

	public class TwilioInboundForm
	{
		public string? From { get; set; }      // ex: "whatsapp:+551199..."
		public string? To { get; set; }        // ex: "whatsapp:+14155238886"
		public string? Body { get; set; }
		public string? MessageSid { get; set; } // id da mensagem no Twilio
	}
}
