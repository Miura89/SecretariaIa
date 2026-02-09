using SecretariaIa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.Exceptions
{
	public class CommandResponseException : Exception
	{
		public CommandResponseException(CommandResponse response)
		{
			Status = (int)response.Status;
			Errors = response.Notifications ?? Enumerable.Empty<string>();
		}
		public CommandResponseException(int status, IEnumerable<string>? errors)
		{
			Status = status;
			Errors = errors ?? [];
		}

		public int Status { get; init; }
		public IEnumerable<string> Errors { get; init; }
	}
}
