using Dapper;
using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.IdentityUserQueries
{
	public class GetIdentityUserById : IRequest<IdentityUserDTO?>
	{
		public Guid Id { get; set; }

		public GetIdentityUserById(Guid id)
		{
			Id = id;
		}
	}
	public class GetIdentityUserByIdHandler : IRequestHandler<GetIdentityUserById, IdentityUserDTO?>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactory;

		public GetIdentityUserByIdHandler(IConnectionSqlFactory connectionSqlFactory)
		{
			_connectionSqlFactory = connectionSqlFactory;
		}

		public async Task<IdentityUserDTO?> Handle(GetIdentityUserById request, CancellationToken cancellationToken)
		{
			using var conn = _connectionSqlFactory.CreateConnection();
			await conn.OpenAsync(cancellationToken);

			const string QUERY = @"Select 
									i.[Id], 
									i.[Name], 
									i.[Email], 
									i.[Phone], 
									i.[Type], 
									i.[Role], 
									i.[Country],
									i.[CreatedAt], 
									i.[CreatedBy], 
									i.[UpdatedAt], 
									i.[UpdatedBy]
								FROM [IdentityUser] i
								WHERE i.[Id] = @Id";
			var parameters = new DynamicParameters();
			parameters.Add("Id", request.Id);

			return await conn.QueryFirstOrDefaultAsync<IdentityUserDTO?>(SqlNormalizer.PostgreSQLQuery(QUERY), parameters);
		}
	}
}
