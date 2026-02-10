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
		public IdentityUser(string name, string email, string password, string phone, TypeUser type, Roles role, string formatedPhone, Country country)
		{
			SetName(name);
			SetEmail(email);
			SetPassword(password);
			SetPhone(phone);
			SetTypeUser(type);
			SetRole(role);
			SetFormattedPhone(formatedPhone);
			SetCountry(country);

			Validate();
		}

		public string Name { get; private set; } = string.Empty;
		public string Email { get; private set; } = string.Empty;
		public string Password { get; private set; } = string.Empty;
		public string Phone { get; private set; } = string.Empty;
		public string FormatedPhone { get; private set; } = string.Empty;
		public Country Country { get; private set; }
		public TypeUser Type { get; private set; }
		public Roles Role { get; private set; }
		public Profile? Profile { get; set; }
		public ICollection<MessageLog>? MessageLogs { get; set; }
		public ICollection<Expenses>? Expenses { get; set; }
		public Plan? Plan { get; set; }
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
		public IdentityUser SetRole(Roles role)
		{
			Role = role;
			return this;
		}
		public IdentityUser SetFormattedPhone(string formatedPhone)
		{
			FormatedPhone = formatedPhone;
			return this;
		}
		public IdentityUser SetCountry(Country country)
		{
			Country = country;
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
