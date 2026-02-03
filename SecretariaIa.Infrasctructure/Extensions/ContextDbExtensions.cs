using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecretariaIa.Common.Interfaces;
using SecretariaIa.Infrasctructure.Data;
using SecretariaIa.Infrasctructure.Data.EF;
using SecretariaIa.Infrasctructure.Data.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Infrasctructure.Extensions
{
	public static class ContextDbExtensions
	{
		public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultDB")));
			services.AddScoped<IConnectionSqlFactory, ConnectionSqlFactory>();
			services.AddHttpClient<IOpenAiService, OpenAiService>();
			services.AddScoped<ITwilioWhatsAppSender, TwilioWhatsAppSender>();
			return services;
		}
	}
}
