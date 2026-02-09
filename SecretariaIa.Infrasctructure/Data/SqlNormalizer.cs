using SecretariaIa.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SecretariaIa.Infrasctructure.Data
{
	public static partial class SqlNormalizer
	{
		public static SqlParts PostgreSQLQuery(SqlParts parts)
		{
			if (parts is null) throw new ArgumentNullException(nameof(parts));

			return new SqlParts
			{
				Select = PostgreSQLQuery(parts.Select),
				FromWhere = PostgreSQLQuery(parts.FromWhere),
				OrderBy = PostgreSQLQuery(parts.OrderBy)
			};
		}

		public static string PostgreSQLQuery(string query)
		{
			if (string.IsNullOrWhiteSpace(query)) return query;

			// [t.Id] -> t."Id"
			query = BracketedAliasColumnRegex().Replace(query, @"$1.""$2""");

			// t.[Id] -> t."Id"
			query = AliasBracketedColumnRegex().Replace(query, @"$1.""$2""");

			// [Id] -> "Id"
			query = ColumnsRegex().Replace(query, @"""$2""");

			return query;
		}

		// [t.Id]
		[GeneratedRegex(@"\[([a-z_][a-z_0-9]*)\.([a-z_][a-z_0-9]*)\]", RegexOptions.IgnoreCase)]
		private static partial Regex BracketedAliasColumnRegex();

		// t.[Id]
		[GeneratedRegex(@"\b([a-z_][a-z_0-9]*)\.\[([a-z_][a-z_0-9]*)\]", RegexOptions.IgnoreCase)]
		private static partial Regex AliasBracketedColumnRegex();

		// [Id]
		[GeneratedRegex(@"(\[([a-z_][a-z_0-9]*)\])", RegexOptions.IgnoreCase)]
		private static partial Regex ColumnsRegex();
	}
}
