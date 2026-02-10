using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Infrasctructure.Data.EF.Configuration;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SecretariaIa.Infrasctructure.Data.EF
{
	public class ApplicationContext : DbContext, IUnitOfWork
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{
		}

		public async Task<bool> Commit(CancellationToken cancellationToken = default)
		{
			return await base.SaveChangesAsync(cancellationToken) > 0;
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration<Expenses>(new ExpensesConfiguration());
			modelBuilder.ApplyConfiguration<MessageLog>(new MessageLogConfiguration());
			modelBuilder.ApplyConfiguration<IdentityUser>(new IdentityUserConfiguration());
			modelBuilder.ApplyConfiguration<Profile>(new ProfileConfiguration());
			modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
			modelBuilder.ApplyConfiguration<OpenAiUsageLog>(new OpenAiUsageConfiguration());
			modelBuilder.ApplyConfiguration<Subscription>(new SubscriptionEntityTypeConfiguration());
			modelBuilder.Ignore<Notification>();
		}
	}
}
