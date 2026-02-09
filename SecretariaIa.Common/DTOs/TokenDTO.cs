using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.DTOs
{
	public record TokenDTO(string Token, string RefreshToken, DateTime ExpiresAt, string Type = "Bearer");
	public record RefreshTokenRequest(string RefreshToken);
}
