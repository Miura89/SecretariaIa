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
	public class ExpensesConfiguration : IEntityTypeConfiguration<Expenses>
	{
		public void Configure(EntityTypeBuilder<Expenses> builder)
		{
			builder.ToTable("Expenses");
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Amount).IsRequired();
			builder.Property(x => x.Description);
			builder.Property(x => x.OccureedAt).IsRequired();
			builder.Property(x => x.Category).IsRequired();
			builder.Property(x=>x.Currency).IsRequired();

			builder.HasOne(x => x.IdentityUser).WithMany(x => x.Expenses).HasForeignKey(x => x.IdentityUserId);
		}
	}
}
