using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.DTOs
{
	public class TokenConfigDTO
	{
		public required int MinutesToExpiresToken { get; init; }
		public required int MinutesToExpiresRefreshToken { get; init; }
		public required int MinutesToExpiresRecoveryToken { get; init; }
		public required string SecretKey { get; init; }
		public required Uri Issuer { get; init; }
		public required Uri Audience { get; init; }
	}
}
