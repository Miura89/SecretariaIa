using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Entities
{
	public class OpenAiUsageLog : Entity<Guid>
	{
		public OpenAiUsageLog()
		{

		}

		public OpenAiUsageLog(string requestId, string model, int promptTokens, int completionTokens, int totalTokens, decimal costUsd, DateTime timestamp, int durationMs, bool success, string? errorMessage, string operationType, string inputType, Guid? identityUserId, IdentityUser identityUser, Subscription subscription, Guid subscriptionId, Plan plan, Guid planId, int? inputCharacters, int? outputCharacters, string? promptVersion)
		{
			RequestId = requestId;
			Model = model;
			PromptTokens = promptTokens;
			CompletionTokens = completionTokens;
			TotalTokens = totalTokens;
			CostUsd = costUsd;
			Timestamp = timestamp;
			DurationMs = durationMs;
			Success = success;
			ErrorMessage = errorMessage;
			OperationType = operationType;
			InputType = inputType;
			IdentityUserId = identityUserId;
			IdentityUser = identityUser;
			Subscription = subscription;
			SubscriptionId = subscriptionId;
			Plan = plan;
			PlanId = planId;
			InputCharacters = inputCharacters;
			OutputCharacters = outputCharacters;
			PromptVersion = promptVersion;
		}

		public string RequestId { get; set; } = default!;
		public string Model { get; set; } = default!;
		// Tokens
		public int PromptTokens { get; set; }
		public int CompletionTokens { get; set; }
		public int TotalTokens { get; set; }
		public decimal CostUsd { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
		// Performance
		public int DurationMs { get; set; }
		public bool Success { get; set; }
		public string? ErrorMessage { get; set; }
		public string OperationType { get; set; }
		public string InputType { get; set; }
		// Contexto de negócio
		public Guid? IdentityUserId { get; set; }
		public IdentityUser IdentityUser { get; set; }
		public Subscription Subscription { get; set; }
		public Guid SubscriptionId { get; set; }
		public Plan Plan { get; set; }
		public Guid PlanId { get; set; }

		// Métricas adicionais
		public int? InputCharacters { get; set; }
		public int? OutputCharacters { get; set; }
		public string? PromptVersion { get; set; }

	}
}
