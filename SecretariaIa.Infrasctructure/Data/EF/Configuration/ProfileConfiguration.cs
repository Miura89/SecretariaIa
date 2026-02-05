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
	public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
	{
		public void Configure(EntityTypeBuilder<Profile> builder)
		{
			builder.ToTable("Profile");
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Currency);
			builder.Property(x => x.TimeZone);
			builder.Property(x => x.Language);
			builder.Property(x => x.MonthlyBudget);

			builder.HasOne(x => x.IdentityUser).WithOne(x => x.Profile).HasForeignKey<Profile>(x => x.IdentityUserId);
		}
	}
}
