using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Infrasctructure.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Infrasctructure.Data.Repositories
{
	public class MessagesLogsRepository : Repository<Guid, MessageLog>, IMessagesLogsRepository
	{
		public MessagesLogsRepository(ApplicationContext context) : base(context)
		{
		}
	}
}
