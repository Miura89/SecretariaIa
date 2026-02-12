using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaIa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Infrasctructure.Data.EF.Configuration
{
	public class OpenAiUsageConfiguration : IEntityTypeConfiguration<OpenAiUsageLog>
	{
		public void Configure(EntityTypeBuilder<OpenAiUsageLog> builder)
		{
			builder.ToTable("OpenAiUsageLogs");

			// Primary Key
			builder.HasKey(x => x.Id);

			// ===== PROPRIEDADES BÁSICAS =====

			builder.Property(x => x.RequestId)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(x => x.Model)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(x => x.OperationType)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(x => x.InputType)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(x => x.ErrorMessage)
				.HasMaxLength(1000);

			builder.Property(x => x.PromptVersion)
				.HasMaxLength(50);

			builder.Property(x => x.Timestamp)
				.IsRequired();

			builder.Property(x => x.CostUsd)
				.HasColumnType("decimal(18,6)"); // precisão importante p/ custo IA

			// ===== TOKENS =====

			builder.Property(x => x.PromptTokens)
				.IsRequired();

			builder.Property(x => x.CompletionTokens)
				.IsRequired();

			builder.Property(x => x.TotalTokens)
				.IsRequired();

			builder.Property(x => x.DurationMs)
				.IsRequired();

			builder.Property(x => x.Success)
				.IsRequired();

			// ===== RELACIONAMENTOS =====

			builder.HasOne(x => x.IdentityUser)
				.WithMany()
				.HasForeignKey(x => x.IdentityUserId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Subscription)
				.WithMany()
				.HasForeignKey(x => x.SubscriptionId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(x => x.Plan)
				.WithMany()
				.HasForeignKey(x => x.PlanId)
				.OnDelete(DeleteBehavior.Restrict);

			// ===== ÍNDICES (MUITO IMPORTANTE) =====

			builder.HasIndex(x => x.Timestamp);

			builder.HasIndex(x => x.IdentityUserId);

			builder.HasIndex(x => x.SubscriptionId);

			builder.HasIndex(x => x.PlanId);

			builder.HasIndex(x => x.OperationType);

			builder.HasIndex(x => x.Success);

			builder.HasIndex(x => new { x.IdentityUserId, x.Timestamp });
		}
	}
}
