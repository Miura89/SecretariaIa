using Dapper;
using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.ExpensesUserQueries
{
	public class GetExpensesByDayQuery : IRequest<IEnumerable<ExpensesDTO>>
	{
		public string Phone { get; set; }

		public GetExpensesByDayQuery(string phone)
		{
			Phone = phone;
		}

		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}
	public class GetExpensesByDayQueryHandler : IRequestHandler<GetExpensesByDayQuery, IEnumerable<ExpensesDTO>>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactoy;

		public GetExpensesByDayQueryHandler(IConnectionSqlFactory connectionSqlFactoy)
		{
			_connectionSqlFactoy = connectionSqlFactoy;
		}

		public async Task<IEnumerable<ExpensesDTO>> Handle(GetExpensesByDayQuery request, CancellationToken cancellationToken)
		{
			using var conn = _connectionSqlFactoy.CreateConnection();
			await conn.OpenAsync(cancellationToken);

			var identityId = @"SELECT i.[Id] FROM [IdentityUser] i WHERE i.[Phone] = @Phone AND i.[Type] = 2";

			var id = await conn.QueryFirstOrDefaultAsync<Guid>(SqlNormalizer.PostgreSQLQuery(identityId), new { Phone = request.Phone.Replace("whatsapp:", "")});

			var QUERY = @"SELECT e.[Id], e.[Amount] as Value, e.[OccureedAt] as Date, e.[Category], e.[Description] from [Expenses] e where e.[IdentityUserId] = @IdentityUserId";

			var parameters = new DynamicParameters();

			parameters.Add("IdentityUserId", id);

			if(request.StartDate.HasValue)
			{
				QUERY += " AND e.[OccureedAt] >= @StartDate";
				parameters.Add("StartDate", request.StartDate.Value);
			}
			if(request.EndDate.HasValue)
			{
				QUERY += " AND e.[OccureedAt] <= @EndDate";
				parameters.Add("EndDate", request.EndDate.Value);
			}

			return await conn.QueryAsync<ExpensesDTO>(SqlNormalizer.PostgreSQLQuery(QUERY), parameters);
		}
	}
}
