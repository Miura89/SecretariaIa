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
	public class MessageLogConfiguration : IEntityTypeConfiguration<MessageLog>
	{
		public void Configure(EntityTypeBuilder<MessageLog> builder)
		{
			builder.ToTable("MessagesLog");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.From);
			builder.Property(x => x.To);
			builder.Property(x => x.Command);
			builder.Property(x => x.ParsedJson);
			builder.Property(x => x.Confidence);
			builder.Property(x => x.Status);
			builder.Property(x => x.ReceivedAt);
			builder.Property(x => x.NeedsClarification);
			builder.HasOne(x => x.IdentityUser).WithMany(x => x.MessageLogs).HasForeignKey(x => x.IdentityUserId);
		}
	}
}
