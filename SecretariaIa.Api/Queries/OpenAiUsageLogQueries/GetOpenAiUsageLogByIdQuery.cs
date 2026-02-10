using Dapper;
using MediatR;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.OpenAiUsageLogQueries
{
	public class GetOpenAiUsageLogByIdQuery : IRequest<OpenAiUsageDTO>
	{
		public string RequestId { get; set; }

		public GetOpenAiUsageLogByIdQuery(string requestId)
		{
			RequestId = requestId;
		}
	}
	public class GetOpenAiUsageLogByIdQueryHandler : IRequestHandler<GetOpenAiUsageLogByIdQuery, OpenAiUsageDTO>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactory;

		public GetOpenAiUsageLogByIdQueryHandler(IConnectionSqlFactory connectionSqlFactory)
		{
			_connectionSqlFactory = connectionSqlFactory;
		}

		public async Task<OpenAiUsageDTO> Handle(GetOpenAiUsageLogByIdQuery request, CancellationToken cancellationToken)
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
									log.[Timestamp]
									FROM [OpenAiUsageLog] log
									WHERE log.[RequestId] = @RequestId";
			var parameters = new DynamicParameters();

			parameters.Add("RequestId", request.RequestId);

			return await conn.QueryFirstOrDefaultAsync<OpenAiUsageDTO>(SqlNormalizer.PostgreSQLQuery(QUERY), parameters);
		}
	}
}
