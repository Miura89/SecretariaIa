using Dapper;
using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.IdentityUserQueries
{
	public class GetIdentityUserByEmail : IRequest<IdentityUserDTO?>
	{
		public string Email { get; set; }

		public GetIdentityUserByEmail(string email)
		{
			Email = email;
		}
	}
	public class GetIdentityUserByEmailHandler : IRequestHandler<GetIdentityUserByEmail, IdentityUserDTO?>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactory;

		public GetIdentityUserByEmailHandler(IConnectionSqlFactory connectionSqlFactory)
		{
			_connectionSqlFactory = connectionSqlFactory;
		}

		public async Task<IdentityUserDTO?> Handle(GetIdentityUserByEmail request, CancellationToken cancellationToken)
		{
			using var conn = _connectionSqlFactory.CreateConnection();
			await conn.OpenAsync(cancellationToken);

			const string QUERY = @"SELECT i.[Id], i.[Name], i.[Type], i.[Role], i.[Email] FROM [IdentityUser] i WHERE i.[Email] = @Email";

			var parameters = new DynamicParameters();
			parameters.Add("Email", request.Email);

			return await conn.QueryFirstOrDefaultAsync<IdentityUserDTO?>(SqlNormalizer.PostgreSQLQuery(QUERY), parameters);
		}
	}
}
