using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.DTOs
{
	public class Response
	{
		public Response() { }
		public Response(int status, bool success, IEnumerable<string>? messages)
		{
			Status = status;
			Success = success;
			Messages = messages;
		}

		public int Status { get; set; }
		public bool Success { get; set; }
		public IEnumerable<string>? Messages { get; set; }
	}
}
