using SecretariaIa.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.Interfaces
{
	public interface IOpenAiService
	{
		Task<AiParsedResult> ParseMessage(string message, string examplesJson);
	}
}
