using Dapper;
using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Domain.Enums;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.IdentityUserQueries
{
	public class SearchIdentityUser : IRequest<PagedResult<IdentityUserDTO>>
	{
		public int Type { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public Roles? Role { get; set; }
		public int? Page { get; set; } = 1;
		public int? Limit { get; set; } = 10;
	}
	public class SearchIdentityUserHandler : IRequestHandler<SearchIdentityUser, PagedResult<IdentityUserDTO>>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactory;

		public SearchIdentityUserHandler(IConnectionSqlFactory connectionSqlFactory)
		{
			_connectionSqlFactory = connectionSqlFactory;
		}

		public async Task<PagedResult<IdentityUserDTO>> Handle(SearchIdentityUser request, CancellationToken cancellationToken)
		{
			using var conn = _connectionSqlFactory.CreateConnection();
			await conn.OpenAsync();

			var parameters = new DynamicParameters();

			var parts = new SqlParts
			{
				Select = @" 
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
							i.[UpdatedBy] ",
				FromWhere = @" FROM [IdentityUser] i
								WHERE i.[Type] = @Type ",
				OrderBy = " i.[CreatedAt] desc "
			};
			parameters.Add("@Type", request.Type);

			if (!string.IsNullOrWhiteSpace(request.Name))
			{
				parts.FromWhere += " AND i.[Name] LIKE @Name";
				parameters.Add("@Name", $"%{request.Name}%");
			}
			if (!string.IsNullOrWhiteSpace(request.Email))
			{
				parts.FromWhere += " AND i.[Email] LIKE @Email";
				parameters.Add("@Email", $"%{request.Email}%");
			}
			if (!string.IsNullOrWhiteSpace(request.Phone))
			{
				parts.FromWhere += " AND i.[Phone] LIKE @Phone";
				parameters.Add("@Phone", $"%{request.Phone}%");
			}
			if (request.Role.HasValue)
			{
				parts.FromWhere += " AND i.[Role] = @Role";
				parameters.Add("@Role", request.Role.Value);
			}

			var pgParts = SqlNormalizer.PostgreSQLQuery(parts);
			return await conn.QueryPagedAsync<IdentityUserDTO>(pgParts, parameters, new PageRequest { Page = request.Page ?? 1, Limit = request.Limit ?? 10 });
		}
	}
}
