using Dapper;
using SecretariaIa.Common.DTOs;
using System.Data;

namespace SecretariaIa.Api.Queries
{
	public static class DapperPagingExtensions
	{
		public static async Task<PagedResult<T>> QueryPagedAsync<T>(
			this IDbConnection conn,
			SqlParts parts,
			object? parameters,
			PageRequest req,
			CancellationToken ct = default)
		{
			var page = req.Page < 1 ? 1 : req.Page;
			var limit = req.Limit is <= 0 or > 200 ? 20 : req.Limit;
			var offset = (page - 1) * limit;

			var countSql = $@"select count(*) {parts.FromWhere};";
			var pageSql = $@"
            select {parts.Select}
            {parts.FromWhere}
            order by {parts.OrderBy}
            limit @Limit offset @Offset;";

			using var multi = await conn.QueryMultipleAsync(
				$"{countSql}\n{pageSql}",
				new DynamicParameters(parameters)
					.AddParam("Limit", limit)
					.AddParam("Offset", offset),
				commandTimeout: 60);

			var total = await multi.ReadFirstAsync<long>();
			var items = (await multi.ReadAsync<T>()).ToList();
			var totalPages = total == 0 ? 1 : (int)Math.Ceiling(total / (double)limit);

			return new PagedResult<T>
			{
				Items = items,
				Page = page,
				Limit = limit,
				Total = total,
				TotalPages = totalPages
			};
		}
		private static DynamicParameters AddParam(this DynamicParameters dp, string name, object? value)
		{
			dp ??= new DynamicParameters();
			dp.Add(name, value);
			return dp;
		}
	}
}
