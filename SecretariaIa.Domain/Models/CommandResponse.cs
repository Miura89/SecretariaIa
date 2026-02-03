using SecretariaIa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Domain.Models
{
	public class CommandResponse
	{
		public bool Success { get => Notifications is null || Notifications.Count == 0; }
		public List<string>? Notifications { get; private set; }
		public StatusNotification Status { get; private set; }

		public CommandResponse AddNotifications(params string[] notifications)
		{
			Status = StatusNotification.BAD_REQUEST;
			AddMessages(notifications);

			return this;
		}

		public CommandResponse AddNotifications(StatusNotification status, params string[] notifications)
		{
			Status = status;
			AddMessages(notifications);

			return this;
		}

		internal CommandResponse AddNotifications(object value)
		{
			throw new NotImplementedException();
		}

		private void AddMessages(params string[] notifications)
		{
			foreach (var notification in notifications)
			{
				Notifications ??= [];
				Notifications.Add(notification);
			}
		}
	}
}
