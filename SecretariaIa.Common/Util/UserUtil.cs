using SecretariaIa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.Util
{
	public static class UserUtil
	{
		public static string? GetClaimValue(this ClaimsPrincipal user, string type)
		{
			return user?.Claims?.FirstOrDefault(c => c.Type.Equals(type, StringComparison.OrdinalIgnoreCase))?.Value;
		}
		public static Roles? GetRole(this ClaimsPrincipal user)
		{
			return user?.GetClaimValue(ClaimTypes.Role) switch
			{
				"Master" => Roles.Master,
				"Operator" => Roles.Operator,
				"Viewer" => Roles.Viewer,
				"Customer" => Roles.Customer,
				_ => null
			};
		}
		public static string? GetPhone(this ClaimsPrincipal user)
		{
			return user?.GetClaimValue("phone") ?? string.Empty;
		}
		public static Guid? GetId(this ClaimsPrincipal user)
		{
			return user.GetClaimValue(ClaimTypes.NameIdentifier) switch
			{
				var id when Guid.TryParse(id, out var parsedId) => parsedId,
				_ => null
			};
		}
		public static string? GetEmail(this ClaimsPrincipal user)
		{
			return user?.GetClaimValue(ClaimTypes.Email) ?? string.Empty;
		}
	}
}
