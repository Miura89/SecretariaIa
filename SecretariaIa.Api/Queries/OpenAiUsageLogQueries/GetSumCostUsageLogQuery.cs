using Dapper;
using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Common.Interfaces;
using SecretariaIa.Infrasctructure.Data;
using System.Threading;
using System.Threading.Tasks;

public class GetOpenAiUsageLogSumCostQuery : IRequest<OpenAiUsageLogSumCostDTO>
{
}

public class GetOpenAiUsageLogSumCostQueryHandler : IRequestHandler<GetOpenAiUsageLogSumCostQuery, OpenAiUsageLogSumCostDTO>
{
	private readonly IConnectionSqlFactory _connectionSqlFactory;

	public GetOpenAiUsageLogSumCostQueryHandler(IConnectionSqlFactory connectionSqlFactory)
	{
		_connectionSqlFactory = connectionSqlFactory;
	}

	public async Task<OpenAiUsageLogSumCostDTO> Handle(GetOpenAiUsageLogSumCostQuery request, CancellationToken cancellationToken)
	{
		using var conn = _connectionSqlFactory.CreateConnection();
		await conn.OpenAsync(cancellationToken);

		const string QUERY = @"
            SELECT 
                COALESCE(SUM(log.""CostUsd""), 0) AS ""SumCostUsd""
            FROM ""OpenAiUsageLogs"" log
        ";

		var sum = await conn.QueryFirstAsync<OpenAiUsageLogSumCostDTO>(QUERY);
		return sum;
	}
}
