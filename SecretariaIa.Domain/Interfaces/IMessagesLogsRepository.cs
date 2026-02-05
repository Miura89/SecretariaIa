using SecretariaIa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Interfaces
{
	public interface IMessagesLogsRepository : IRepositoryRead<Guid, MessageLog>, IRepositoryWrite<Guid, MessageLog>
	{
	}
}
