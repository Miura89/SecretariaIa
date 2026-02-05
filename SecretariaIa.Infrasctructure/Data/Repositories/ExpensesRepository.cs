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
	public class ExpensesRepository : Repository<Guid, Expenses>, IExpensesRepository
	{
		public ExpensesRepository(ApplicationContext context) : base(context)
		{
		}
	}
}
