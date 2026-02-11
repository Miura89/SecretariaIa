using SecretariaIa.Common.DTOs;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.Interfaces
{
	public interface IOpenAiService
	{
		Task<AiParsedResult> ParseMessage(string message, string examplesJson, Plan? plan);
		Task<string> TranscribeAudio(Stream audioStream);
		Task<AiParsedResult> ParseAudio(Stream audio, string examplesJson,Plan? plan);
	}
}
