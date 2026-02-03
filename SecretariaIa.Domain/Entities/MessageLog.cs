using SecretariaIa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SecretariaIa.Domain.Entities
{
	public class MessageLog : Entity<Guid>
	{
		public MessageLog()
		{
			
		}
		public MessageLog(string from, string to, DateTimeOffset receivedAt, CommandsMessage command, string parsedJson, decimal confidence, StatusMessage status, bool needsClarifications, IdentityUser identityUser, Guid identityUserId)
		{
			SetFrom(from);
			SetTo(to);
			SetCommand(command);
			SetParsedJson(parsedJson);
			SetConfidence(confidence);
			SetStatus(status);
			SetReceivedAt(receivedAt);
			SetNeedsClarification(needsClarifications);
			SetIdentityUser(identityUser, identityUserId);

			Validate();
		}
		public IdentityUser IdentityUser { get; set; }
		public Guid IdentityUserId { get; set; }
		public string From { get; private set; } = string.Empty;
		public string To { get; private set; } = string.Empty;
		public DateTimeOffset ReceivedAt { get; set; }
		public CommandsMessage Command { get; private set; }
		public string ParsedJson { get; private set; } = "{}";
		public decimal Confidence { get; set; }
		public StatusMessage Status { get; private set; }
		public bool NeedsClarification { get; private set; }

		public MessageLog SetNeedsClarification(bool needsClarification)
		{
			NeedsClarification = needsClarification;
			return this;
		}
		public MessageLog SetIdentityUser(IdentityUser identityUser, Guid identityUserId)
		{
			IdentityUser = identityUser;
			IdentityUserId = identityUserId;
			return this;
		}
		public MessageLog SetFrom(string from)
		{
			From = from;
			return this;
		}
		public MessageLog SetTo(string to) {
			To = to;
			return this;
		}
		public MessageLog SetReceivedAt(DateTimeOffset receivedAt)
		{
			ReceivedAt = receivedAt;
			return this;
		}
		public MessageLog SetCommand(CommandsMessage command)
		{
			Command = command;
			return this;
		}
		public MessageLog SetParsedJson(string parsedJson)
		{
			ParsedJson = parsedJson;
			return this;
		}
		public MessageLog SetStatus(StatusMessage status)
		{
			Status = status;
			return this;
		}
		public MessageLog SetConfidence(decimal confidence)
		{
			Confidence = confidence;
			return this;
		}
		public override bool Validate()
		{
			Clear();
			return IsValid;
		}
	}
}
