using Dapper;
using MediatR;
using SecretariaIa.Api.DTOs;
using SecretariaIa.Infrasctructure.Data;

namespace SecretariaIa.Api.Queries.SubscriptionQueries
{
	public class GetSubscriptionByIdentityUserIdQuery : IRequest<IEnumerable<SubscriptionDTO>>
	{
		public Guid? IdentityUserId { get; set; }

		public GetSubscriptionByIdentityUserIdQuery(Guid? identityUserId)
		{
			IdentityUserId = identityUserId;
		}
	}
	public class GetSubcriptionByIdentityUserIdQueryHandler : IRequestHandler<GetSubscriptionByIdentityUserIdQuery, IEnumerable<SubscriptionDTO>>
	{
		private readonly IConnectionSqlFactory _connectionSqlFactory;

		public GetSubcriptionByIdentityUserIdQueryHandler(IConnectionSqlFactory connectionSqlFactory)
		{
			_connectionSqlFactory = connectionSqlFactory;
		}

		public async Task<IEnumerable<SubscriptionDTO>> Handle(GetSubscriptionByIdentityUserIdQuery request, CancellationToken cancellationToken)
		{
			using var conn = _connectionSqlFactory.CreateConnection();
			await conn.OpenAsync(cancellationToken);

			const string QUERY = @"SELECT 
									s.[Id], 
									i.[Name] as identityUserName, 
									i.[Phone], 
									p.[PlanName], 
									s.[StartDate], 
									s.[EndDate], 
									s.[TrialStartDate], 
									s.[TrialEndDate],	
									s.[Status], 
									s.[CreatedBy] 
								FROM [Subscriptions] s 
								INNER JOIN [IdentityUser] i ON i.[Id] = @IdentityUserId
								INNER JOIN [Plan] p ON p.[Id] = s.[PlanId]
								WHERE s.[IdentityUserId] = @IdentityUserId 
								ORDER BY s.[CreatedAt] desc";

			var parameters = new DynamicParameters();

			parameters.Add("IdentityUserId", request.IdentityUserId);

			return await conn.QueryAsync<SubscriptionDTO>(SqlNormalizer.PostgreSQLQuery(QUERY), parameters);
		}
	}
}
