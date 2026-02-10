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
	public class OpenAiUsageLogRepository : Repository<Guid, OpenAiUsageLog>, IOpenAiUsageLogRepository
	{
		public OpenAiUsageLogRepository(ApplicationContext context) : base(context)
		{
		}
	}
}
