using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.DTOs
{
	public record TwilioInboundDto(
		string From,
		string To,
		string Body,
		string MessageSid,
		string? MediaUrl0 = null,          // URL do áudio ou mídia
		string? MediaContentType0 = null   // tipo do arquivo, ex: audio/ogg
	);

}
