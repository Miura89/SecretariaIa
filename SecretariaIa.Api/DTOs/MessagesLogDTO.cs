using SecretariaIa.Domain.Enums;

namespace SecretariaIa.Api.DTOs
{
	public class MessagesLogDTO 
	{
		public Guid Id { get; set; }
		public string From { get; set; } = string.Empty;
		public string To { get; set; } = string.Empty;
		public DateTime ReceivedAt { get; set; }
		public string ReceivedAtFormatted => ReceivedAt.ToString("dd/MM/yyyy HH:mm:ss");
		public CommandsMessage Command { get; set; }
		public string ParsedJson { get; set; } = string.Empty;
		public double Confidence { get; set; }
		public StatusMessage Status { get; set; }
		public bool NeedsClarification { get; set; }
		public string IdentityUserName { get; set; } = string.Empty;
	}
}
