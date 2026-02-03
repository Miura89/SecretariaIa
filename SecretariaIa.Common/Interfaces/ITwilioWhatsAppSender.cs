using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.Interfaces
{
	public interface ITwilioWhatsAppSender
	{
		Task<string> SendAsync(string toPhoneE164, string text);
	}
}
