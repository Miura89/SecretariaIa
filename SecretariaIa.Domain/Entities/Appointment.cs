using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Entities
{
	public class Appointment : Entity<Guid>
	{
		public Appointment()
		{
			
		}
		public Appointment(Guid identityUserId, IdentityUser identityUser, string title, DateTime scheduledAt, int? remindBeforeMinutes, bool reminderSent)
		{
			IdentityUserId = identityUserId;
			IdentityUser = identityUser;
			Title = title;
			ScheduledAt = scheduledAt;
			RemindBeforeMinutes = remindBeforeMinutes;
			ReminderSent = reminderSent;
		}

		public Guid IdentityUserId { get; set; }
		public IdentityUser IdentityUser { get; set; }
		public string Title { get; set; }
		public DateTime ScheduledAt { get; set; }
		public int? RemindBeforeMinutes { get; set; }
		public bool ReminderSent { get; set; }

	}
}
