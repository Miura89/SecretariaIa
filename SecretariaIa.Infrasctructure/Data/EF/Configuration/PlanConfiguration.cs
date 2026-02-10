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
	public class PlanConfiguration : IEntityTypeConfiguration<Plan>
	{
		public void Configure(EntityTypeBuilder<Plan> builder)
		{
			builder.ToTable("Plan");
			builder.HasKey(x => x.Id);
			builder.Property(x => x.PlanName).IsRequired();
			builder.Property(x => x.PlanDescription).IsRequired();
			builder.Property(x => x.PriceUSD).IsRequired();
			builder.Property(x => x.MaxMessagesPerMonth).IsRequired();
			builder.Property(x => x.MaxOpenAiUsdPerMonth).IsRequired();
			builder.Property(x => x.IsActive).IsRequired();
			builder.Property(x => x.LimitBehaviore).IsRequired();
			builder.Property(x => x.DefaultMode).IsRequired();
		}
	}
}
