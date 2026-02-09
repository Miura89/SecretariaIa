using SecretariaIa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.DTOs
{
	public record UserTokenDTO(string Id, string? Email, string Name, Roles Role);
}
