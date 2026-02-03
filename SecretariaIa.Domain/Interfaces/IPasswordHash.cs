using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Interfaces
{
	public interface IPasswordHash
	{
		string? GenerateHash(string password);
		bool Verify(string password, string hash);
	}
}
