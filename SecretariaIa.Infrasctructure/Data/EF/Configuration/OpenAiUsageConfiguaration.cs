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
			builder.ToTable("OpenAiUsageLog"); // Nome da tabela

			builder.HasKey(x => x.RequestId); // PK

			builder.Property(x => x.RequestId)
				.IsRequired()
				.HasMaxLength(50); // Ajuste conforme tamanho esperado

			builder.Property(x => x.Model)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(x => x.PromptTokens)
				.IsRequired();

			builder.Property(x => x.CompletionTokens)
				.IsRequired();

			builder.Property(x => x.TotalTokens)
				.IsRequired();

			builder.Property(x => x.CostUsd)
				.HasColumnType("decimal(18,6)") // Armazena valores decimais corretamente
				.IsRequired();

			builder.Property(x => x.Timestamp)
				.IsRequired();
		}
	}
}
