using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Infrasctructure.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Infrasctructure.Extensions
{
	public static class SecurityExtensions
	{
		public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpContextAccessor();

			string chave = configuration.GetValue<string>("TokenConfig:SecretKey")
				?? throw new ArgumentException("'SecretKey' não pode ser nula ou vazia.");

			services.AddOptions<TokenConfigDTO>()
				.Bind(configuration.GetSection("TokenConfig"))
				.ValidateOnStart();

			services.AddSingleton<IPasswordHash, PasswordHash>();

			//Jwt
			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(opt =>
			{
				opt.RequireHttpsMetadata = false;
				opt.SaveToken = true;
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(chave)),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				};
			});

			services.AddAuthorization();

			return services;
		}
	}
}
