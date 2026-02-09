using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.DTOs
{
	public sealed class PageRequest
	{
		public int Page { get; init; } = 1;    // 1-based
		public int Limit { get; init; } = 20;  // clamp no helper
	}
}
