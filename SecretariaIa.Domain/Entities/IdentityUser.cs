using SecretariaIa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretariaIa.Domain.Entities
{
	public class IdentityUser : Entity<Guid>
	{
		public IdentityUser()
		{
			
		}
		public IdentityUser(string name, string email, string password, string phone, TypeUser type)
		{
			SetName(name);
			SetEmail(email);
			SetPassword(password);
			SetPhone(phone);
			SetTypeUser(type);

			Validate();
		}

		public string Name { get; private set; } = string.Empty;
		public string Email { get; private set; } = string.Empty;
		public string Password { get; private set; } = string.Empty;
		public string Phone { get; private set; } = string.Empty;
		public TypeUser Type { get; private set; }
		public Profile? Profile { get; set; }
		public ICollection<MessageLog>? MessageLogs { get; set; }
		public ICollection<Expenses>? Expenses { get; set; }
		public IdentityUser SetName(string name)
		{
			Name = name;
			return this;
		}
		public IdentityUser SetEmail(string email)
		{
			Email = email;
			return this;
		}
		public IdentityUser SetTypeUser(TypeUser type)
		{
			Type = type;
			return this;
		}
		public IdentityUser SetPassword(string password)
		{
			Password = password;
			return this;
		}
		public IdentityUser SetPhone(string phone)
		{
			Phone = phone;
			return this;
		}

		public override bool Validate()
		{
			Clear();
			return IsValid;
		}
	}
}
