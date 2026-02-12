using Dapper;
using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.OpenAiUsageLogQueries
{
	public class GetOpenAiUsageLogByIdQuery : IRequest<OpenAiUsageLogDTO>
	{
		public string RequestId { get; set; }

		public GetOpenAiUsageLogByIdQuery(string requestId)
		{
			RequestId = requestId;
		}
	}
	public class GetOpenAiUsageLogByIdQueryHandler : IRequestHandler<GetOpenAiUsageLogByIdQuery, OpenAiUsageLogDTO>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactory;

		public GetOpenAiUsageLogByIdQueryHandler(IConnectionSqlFactory connectionSqlFactory)
		{
			_connectionSqlFactory = connectionSqlFactory;
		}

		public async Task<OpenAiUsageLogDTO> Handle(GetOpenAiUsageLogByIdQuery request, CancellationToken cancellationToken)
		{
			using var conn = _connectionSqlFactory.CreateConnection();
			await conn.OpenAsync(cancellationToken);

			const string QUERY = @"	SELECT 
									log.[Id], 
									log.[RequestId], 
									log.[Model], 
									log.[PromptTokens], 
									log.[CompletionTokens], 
									log.[TotalTokens], 
									log.[CostUsd], 
									log.[Timestamp], 
									log.[DurationMs], 
									p.[PlanName], 
									log.[Success], 
									log.[InputType], 
									log.[OperationType],
									log.[PromptVersion],
									log.[OutputCharacters],
									log.[InputCharacters],
									i.[Name] as IdentityName
									FROM [OpenAiUsageLogs] log
									INNER JOIN [Plan] p ON p.[Id] = log.[PlanId]
									INNER JOIN [IdentityUser] i ON i.[Id] = log.[IdentityUserId]
									WHERE log.[RequestId] = @RequestId";
			var parameters = new DynamicParameters();

			parameters.Add("RequestId", request.RequestId);

			return await conn.QueryFirstOrDefaultAsync<OpenAiUsageLogDTO>(SqlNormalizer.PostgreSQLQuery(QUERY), parameters);
		}
	}
}
