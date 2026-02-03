using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerExtension
{
	public static IServiceCollection AddSwagger<TSchemaFilter>(
		this IServiceCollection services,
		IConfiguration configuration,
		string description = "",
		string version = "v1",
		ISchemaFilter? filter = null) where TSchemaFilter : ISchemaFilter
	{
		string title = configuration.GetValue<string>("ApiName") ?? "Agendai.Api";

		return services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = title,
				Version = version,
				Description = description,
				TermsOfService = new Uri("https://example.com/terms"),
				Contact = new OpenApiContact
				{
					Name = "Agendai",
					Email = "support@agendai.com"
				}
			});

			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Insira o token",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "bearer"
			});

			c.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
		});

			if (filter is null)
			{
				c.SchemaFilter<SwaggerFillter>();
			}
			else
			{
				c.SchemaFilterDescriptors.Add(new()
				{
					Type = typeof(TSchemaFilter)
				});
			}
		});
	}
}

public class SwaggerFillter : ISchemaFilter
{
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (schema.Properties.Any())
		{
			if (context.Type.Name.StartsWith("Create") || context.Type.Name.StartsWith("Update") || context.Type.Name.StartsWith("Exlude"))
			{
				string[] properties =
				[
					"id",
						"createdBy",
						"updatedBy",
						"excludedBy"
				];

				foreach (var property in properties)
					schema.Properties.Remove(property);
			}

			if (context.Type.Name.StartsWith("Count") || context.Type.Name.StartsWith("Search"))
				schema.Properties.Remove("tenantId");
		}
	}
}