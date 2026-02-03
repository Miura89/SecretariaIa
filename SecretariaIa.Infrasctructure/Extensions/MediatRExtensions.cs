using Microsoft.Extensions.DependencyInjection;
using SecretariaIa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretariaIa.Infrasctructure.Extensions
{
	public static class MediatRExtension
	{
		public static IServiceCollection AddMediator(this IServiceCollection services, Assembly apiAssembly)
		{
			return services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies([
				apiAssembly,
				typeof(CommandResponse).GetTypeInfo().Assembly
				]));
		}
	}
}
