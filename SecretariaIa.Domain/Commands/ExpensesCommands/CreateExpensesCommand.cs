using MediatR;
using SecretariaIa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Commands.ExpensesCommands
{
	public class CreateExpensesCommand : IRequest<CommandResultResponse<Guid>>
	{

	}
}
