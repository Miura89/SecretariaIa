using Dapper;
using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.OpenAiUsageLogQueries
{
	public class SearchOpenAiUsageLogQuerie : IRequest<PagedResult<OpenAiUsageLogDTO>>
	{
		public int? Page { get; set; }
		public int? Limit { get; set; }
	}
	public class SearchOpenAiUsageLogQuerieHandler : IRequestHandler<SearchOpenAiUsageLogQuerie, PagedResult<OpenAiUsageLogDTO>>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactory;

		public SearchOpenAiUsageLogQuerieHandler(IConnectionSqlFactory connectionSqlFactory)
		{
			_connectionSqlFactory = connectionSqlFactory;
		}

		public async Task<PagedResult<OpenAiUsageLogDTO>> Handle(SearchOpenAiUsageLogQuerie request, CancellationToken cancellationToken)
		{
			using var conn = _connectionSqlFactory.CreateConnection();
			await conn.OpenAsync(cancellationToken);

			var parts = new SqlParts
			{
				Select = "log.[Id], log.[RequestId], log.[Model], log.[PromptTokens], log.[CompletionTokens], log.[TotalTokens], log.[CostUsd], log.[Timestamp]",
				FromWhere = "FROM [OpenAiUsageLog] log",
				OrderBy = "log.[Timestamp] DESC"
			};
			var pgParts = SqlNormalizer.PostgreSQLQuery(parts);

			var parameters = new DynamicParameters();

			return await conn.QueryPagedAsync<OpenAiUsageLogDTO>(pgParts, parameters, new PageRequest { Page = request.Page ?? 1, Limit = request.Limit ?? 8 }, cancellationToken);
		}
	}
}
