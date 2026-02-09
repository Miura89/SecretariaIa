using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.DTOs
{
	public class PagedResult<T>
	{
		public ICollection<T> Items { get; set; } = Array.Empty<T>();
		public int Page { get; init; }
		public int Limit { get; init; }
		public long Total { get; init; }
		public int TotalPages { get; init; }
		public bool HasNext => Page < TotalPages;
		public bool HasPrev => Page > 1;
	}
}
