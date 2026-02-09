using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.DTOs
{
	public sealed class SqlParts
	{
		public required string Select { get; init; }
		public required string FromWhere { get; set; }
		public required string OrderBy { get; init; }
	}
}
