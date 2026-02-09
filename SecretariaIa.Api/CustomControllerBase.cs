using Microsoft.AspNetCore.Mvc;
using SecretariaIa.Common.DTOs;
using SecretariaIa.Common.Util;
using SecretariaIa.Domain.Enums;

namespace SecretariaIa.Api
{
	public class CustomControllerBase : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private static bool IsSucess(int status) => status >= 200 && status <= 299;
		protected ObjectResult Result(int status = 200)
		{
			return StatusCode(status, new Response(status, IsSucess(status), null));
		}
		protected ObjectResult Result<TResult>(TResult result, int status = 200)
		{
			return StatusCode(status, new ResultResponse<TResult>(status, IsSucess(status), null, result));
		}
		protected ObjectResult Error(IEnumerable<string> messages, int statusCode = 400)
		{
			statusCode = IsSucess(statusCode) ? 400 : statusCode;
			return StatusCode(statusCode, new Response(statusCode, false, messages));
		}

		protected string? GetAuthenticatedUserEmail()
		{
			return HttpContext.User.GetEmail();
		}

		protected Guid? GetAuthenticatedUserId()
		{
			return HttpContext.User.GetId();
		}
		protected string? GetAuthenticatedPhone()
		{
			return HttpContext.User.GetPhone();
		}

		protected Roles? GetAuthenticatedRole()
		{
			return HttpContext.User.GetRole();
		}

		protected void CheckMasterRequirement()
		{
			if (HttpContext.User is null)
				throw new UnauthorizedAccessException();
			var role = GetAuthenticatedRole();
			if (role != Roles.Master)
				throw new UnauthorizedAccessException();
		}
		protected void CheckOperatorRequirement()
		{
			if (HttpContext.User is null)
				throw new UnauthorizedAccessException();
			var role = GetAuthenticatedRole();
			if (role != Roles.Master && role != Roles.Operator)
				throw new UnauthorizedAccessException();
		}
		protected void CheckCustomerRequirement()
		{
			if (HttpContext.User is null)
				throw new UnauthorizedAccessException();
			var role = GetAuthenticatedRole();
			if (role != Roles.Customer)
				throw new UnauthorizedAccessException();
		}
	}
}
