using SecretariaIa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Domain.Models
{
	public class CommandResultResponse<T> : CommandResponse
	{
		public T? Result { get; private set; }

		public void SetResult(T result)
		   => Result = result;

		public new CommandResultResponse<T> AddNotifications(StatusNotification status, params string[] notifications)
		{
			base.AddNotifications(status, notifications);
			return this;
		}

		public new CommandResultResponse<T> AddNotifications(params string[] notifications)
		{
			base.AddNotifications(notifications);
			return this;
		}
	}
}
