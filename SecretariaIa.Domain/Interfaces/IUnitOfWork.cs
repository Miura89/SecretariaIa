using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Domain.Interfaces
{
	public interface IUnitOfWork
	{
		Task<bool> Commit(CancellationToken cancellationToken = default);
	}
}
