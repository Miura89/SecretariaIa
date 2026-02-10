using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Enums
{
	public enum PlanLimitBehavior
	{
		Block = 1,        // bloqueia uso
		DegradeModel = 2, // cai para modelo mais barato
		Queue = 3         // entra em fila
	}
}
