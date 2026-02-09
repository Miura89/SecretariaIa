using Dapper;
using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Domain.Enums;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.IdentityUserQueries
{
	public class AllIdentityUser : IRequest<IEnumerable<IdentityUserDTO>>
	{
		public TypeUser Type { get; set; }
	}
	public class AllIdentityUserHandler : IRequestHandler<AllIdentityUser, IEnumerable<IdentityUserDTO>>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactory;

		public AllIdentityUserHandler(IConnectionSqlFactory connectionSqlFactory)
		{
			_connectionSqlFactory = connectionSqlFactory;
		}

		public async Task<IEnumerable<IdentityUserDTO>> Handle(AllIdentityUser request, CancellationToken cancellationToken)
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
								WHERE i.[Type] = @Type
								Order By i.[CreatedAt] desc";

			var parameters = new DynamicParameters();
			parameters.Add("Type", request.Type);

			return await conn.QueryAsync<IdentityUserDTO>(QUERY, parameters);
		}
	}
}
