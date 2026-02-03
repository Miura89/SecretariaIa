using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Infrasctructure.Data.EF
{
	public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
	{
		public ApplicationContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
			optionsBuilder.UseNpgsql(args?.FirstOrDefault());

			return new ApplicationContext(optionsBuilder.Options);
		}
	}
}
