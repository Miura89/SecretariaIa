using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Domain.Utils
{
	internal static class NotificationUtil
	{
		public static string[] GetMessages(this IReadOnlyCollection<Notification> notifications)
			=> notifications.Select(x => $"{x.Message}").ToArray();

		public static string ToKey(this string property)
			=> $"{property}:";

		public static int GetLength(this string? value)
			=> value?.Length ?? 0;
	}
}
