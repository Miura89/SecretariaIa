using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.DTOs
{
	public class ResultResponse<TResult> : Response
	{
		public ResultResponse(int statusCode, bool success, IEnumerable<string>? messages)
   : base(statusCode, success, messages)
		{
		}

		public ResultResponse(int status, bool success, IEnumerable<string>? messages, TResult? result)
			: base(status, success, messages)
		{
			Result = result;
		}

		public TResult? Result { get; set; }
	}
}
