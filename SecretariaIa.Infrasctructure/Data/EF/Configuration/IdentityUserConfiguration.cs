using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Infrasctructure.Data.EF.Configuration
{
	public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
	{
		public void Configure(EntityTypeBuilder<IdentityUser> builder)
		{
			builder.ToTable("IdentityUser");
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name)
				.IsRequired()
				.HasMaxLength(150);
			builder.Property(x => x.Email).IsRequired().HasMaxLength(300);
			builder.Property(x => x.Password).IsRequired();
			builder.Property(x => x.Phone).IsRequired();
			builder.Property(x => x.Type).IsRequired();
			builder.Property(x => x.Role).IsRequired();
			builder.Property(x => x.FormatedPhone).IsRequired(false);
			builder.Property(x => x.Country)
				.HasConversion<int>()
				.HasDefaultValue(Country.BRL);
			builder.HasMany(x => x.MessageLogs).WithOne(x => x.IdentityUser).HasForeignKey(x => x.IdentityUserId);
			builder.HasMany(x => x.Expenses).WithOne(x => x.IdentityUser).HasForeignKey(x => x.IdentityUserId);
		}
	}
}
