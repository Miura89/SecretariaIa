using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Domain.Enums
{
	public enum CommandsMessage
	{
		CreateExpense = 1,
		GetSummary = 2,
		SetBudget = 3,
		UndoLastExpense = 4,
		Help = 5
	}
}
