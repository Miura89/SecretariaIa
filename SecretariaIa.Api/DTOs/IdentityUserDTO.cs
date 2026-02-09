using SecretariaIa.Domain.Enums;

namespace SecretariaIa.Api.DTOs
{
	public class IdentityUserDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public TypeUser Type { get; set; }
		public Roles Role { get; set; }
		public Country Country { get; set; }
	}
}
