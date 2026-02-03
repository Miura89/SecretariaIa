using Flunt.Notifications;
using SecretariaIa.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Domain.Entities
{
	public abstract class Entity<TId> : Notifiable<Notification>
	{
		public Entity() { }

		public virtual TId Id { get; set; } = default!;
		public DateTime CreatedAt { get; set; } = DateTimeNow();
		public DateTime? UpdatedAt { get; private set; }
		public DateTime? ExcludedAt { get; private set; }
		public Guid? CreatedBy { get; private set; }
		public Guid? UpdatedBy { get; private set; }
		public Guid? ExcludedBy { get; private set; }

		public void Create(Guid? id)
		{
			CreatedAt = DateTimeNow();
			CreatedBy = id == Guid.Empty ? null : id;
		}

		public void Update(Guid? id)
		{
			UpdatedAt = DateTimeNow();
			UpdatedBy = id == Guid.Empty ? null : id;
		}

		public void Exclude(Guid? id)
		{
			ExcludedAt = DateTimeNow();
			ExcludedBy = id == Guid.Empty ? null : id;
		}

		public string[] GetNotifications()
		{
			return Notifications.GetMessages();
		}

		public virtual bool Validate() => false;

		private static DateTime DateTimeNow() => DateTime.UtcNow;
	}
}
