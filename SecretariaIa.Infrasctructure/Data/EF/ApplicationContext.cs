using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SecretariaIa.Domain.Interfaces;
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

			modelBuilder.Ignore<Notification>();
		}
	}
}
