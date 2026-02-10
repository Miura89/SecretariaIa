using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Infrasctructure.Data.EF.Configuration
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using SecretariaIa.Domain.Entities;

	public class SubscriptionEntityTypeConfiguration
		: IEntityTypeConfiguration<Subscription>
	{
		public void Configure(EntityTypeBuilder<Subscription> builder)
		{
			builder.ToTable("Subscriptions");

			builder.HasKey(s => s.Id);

	
			builder.HasOne(s => s.IdentityUser)
				.WithMany(x=>x.Subscription)
				.HasForeignKey(s => s.IdentityUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(s => s.Plan)
				.WithMany() 
				.HasForeignKey(s => s.PlanId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(s => s.StartDate)
				.IsRequired();

			builder.Property(s => s.EndDate)
				.IsRequired(false);

			builder.Property(s => s.TrialStartDate)
				.IsRequired(false);

			builder.Property(s => s.TrialEndDate)
				.IsRequired(false);

			builder.Property(s => s.Status)
				.IsRequired()
				.HasConversion<string>()
				.HasMaxLength(20);

			builder.Property(x => x.AmountPaid).IsRequired();


			builder.Ignore(s => s.IsInTrial);
			builder.Ignore(s => s.IsActive);
		}
	}

}
