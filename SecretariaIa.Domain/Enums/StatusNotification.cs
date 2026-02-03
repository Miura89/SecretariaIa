using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Domain.Enums
{
	public enum StatusNotification
	{
		BAD_REQUEST = 400,
		FORBIDDEN = 403,
		NOT_FOUND = 404,
		CONFLICT = 409,
		UNPROCESSABLE_ENTITY = 422,
		UNAUTHORIZED = 404
	}
}
