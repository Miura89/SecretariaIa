using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.MessagesLogMonitoringQueries
{
	public class SearchMessagesLog : IRequest<PagedResult<MessagesLogDTO>>
	{
		public string? Phone { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int? Page { get; set; } = 1;
		public int? Limit { get; set; } = 10;
	}
	public class SearchMessagesLogHandler : IRequestHandler<SearchMessagesLog, PagedResult<MessagesLogDTO>>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactory;

		public SearchMessagesLogHandler(IConnectionSqlFactory connectionSqlFactory)
		{
			_connectionSqlFactory = connectionSqlFactory;
		}

		public async Task<PagedResult<MessagesLogDTO>> Handle(SearchMessagesLog request, CancellationToken cancellationToken)
		{
			using var conn = _connectionSqlFactory.CreateConnection();
			await conn.OpenAsync(cancellationToken);

			var parameters = new DynamicParameters();

			var parts = new SqlParts
			{
				Select = @"m.[Id], m.[From], m.[To], m.[ReceivedAt], m.[Command], m.[ParsedJson], m.[Confidence], m.[Status], m.[NeedsClarification]",
				FromWhere = @" From [MessagesLog] m",
				OrderBy = @"m.[CreatedAt] desc"
			};
			var pgParts = SqlNormalizer.PostgreSQLQuery(parts);
			if (!string.IsNullOrWhiteSpace(request.Phone))
			{
				pgParts.FromWhere += " where m.[From] like @Phone or m.[To] like @Phone";
				parameters.Add("Phone", request.Phone);
			}

			return await conn.QueryPagedAsync<MessagesLogDTO>(pgParts, parameters, new PageRequest { Page = request.Page ?? 1, Limit = request.Limit ?? 10 });
		}
	}
}
