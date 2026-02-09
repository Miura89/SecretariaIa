using Dapper;
using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.MessagesLogMonitoringQueries
{
	public class GetMessagesLogById : IRequest<MessagesLogDTO?>
	{
		public Guid? Id { get; set; }

		public GetMessagesLogById(Guid? id)
		{
			Id = id;
		}
	}
	public class GetMessagesLogByIdHandler : IRequestHandler<GetMessagesLogById, MessagesLogDTO?>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactory;

		public GetMessagesLogByIdHandler(IConnectionSqlFactory connectionSqlFactory)
		{
			_connectionSqlFactory = connectionSqlFactory;
		}

		public async Task<MessagesLogDTO?> Handle(GetMessagesLogById request, CancellationToken cancellationToken)
		{
			using var conn = _connectionSqlFactory.CreateConnection();
			await conn.OpenAsync(cancellationToken);

			const string sql = @"SELECT m.[Id], m.[From], m.[To], m.[ReceivedAt], m.[Command], m.[ParsedJson], m.[Confidence], m.[Status], m.[NeedsClarification] FROM [MessagesLog] m WHERE m.[Id] = @Id";

			var parameters = new DynamicParameters();
			parameters.Add("Id", request.Id);

			return await conn.QueryFirstOrDefaultAsync<MessagesLogDTO?>(sql, parameters);
		}
	}
}
