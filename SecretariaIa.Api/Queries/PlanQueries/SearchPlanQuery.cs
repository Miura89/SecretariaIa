using Dapper;
using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.PlanQueries
{
	public class SearchPlanQuery : IRequest<PagedResult<PlanDTO>>
	{
		public string? PlanName { get; set; }
		public bool? IsActive { get; set; }
		public int? Page { get; set; }
		public int? Limit { get; set; }
	}
	public class SearchPlanQueryHandler : IRequestHandler<SearchPlanQuery, PagedResult<PlanDTO>>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactory;

		public SearchPlanQueryHandler(IConnectionSqlFactory connectionSqlFactory)
		{
			_connectionSqlFactory = connectionSqlFactory;
		}

		public async Task<PagedResult<PlanDTO>> Handle(SearchPlanQuery request, CancellationToken cancellationToken)
		{
			using var conn = _connectionSqlFactory.CreateConnection();
			await conn.OpenAsync(cancellationToken);

			var parts = new SqlParts
			{
				Select = @"p.[Id], p.[PlanName], p.[PlanDescription], p.[PriceUSD], p.[MaxMessagesPerMonth], p.[MaxOpenAiUsdPerMonth], p.[IsActive], p.[LimitBehaviore], p.[DefaultMode]",
				FromWhere = @"FROM [Plan] p",
				OrderBy = @"p.[CreatedAt] desc"
			};
			var parameters = new DynamicParameters();

			if (!string.IsNullOrWhiteSpace(request.PlanName))
			{
				parts.FromWhere += " AND i.[PlanName] ILIKE @PlanName";
				parameters.Add("@PlanName", $"%{request.PlanName}%");
			}
			if(request.IsActive.HasValue)
			{
				parts.FromWhere += " AND i.[IsActive] = true";
			}
			var pgParts = SqlNormalizer.PostgreSQLQuery(parts);

			return await conn.QueryPagedAsync<PlanDTO>(pgParts, parameters, new PageRequest { Page = request.Page ?? 1, Limit = request.Limit ?? 8 });
		}
	}
}
