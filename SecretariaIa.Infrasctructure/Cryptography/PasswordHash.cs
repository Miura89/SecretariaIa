using SecretariaIa.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Infrasctructure.Cryptography
{
	public class PasswordHash : IPasswordHash
	{
		const int WORK_FACTOR = 10;
		public string? GenerateHash(string? password)
		{
			if (password is null) return null;

			return BCrypt.Net.BCrypt.HashPassword(password, workFactor: WORK_FACTOR);
		}

		public bool Verify(string password, string hash)
		{
			return BCrypt.Net.BCrypt.Verify(password, hash);
		}
	}
}
