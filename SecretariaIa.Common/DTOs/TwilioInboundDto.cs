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
		string MessageSid,
		string? Body = "",
		string? MediaUrl0 = null,
		string? MediaContentType0 = null
	);

}
