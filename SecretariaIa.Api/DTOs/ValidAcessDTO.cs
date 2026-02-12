using SecretariaIa.Domain.Entities;

namespace SecretariaIa.Api.DTOs
{
	public class ValidAcessDTO
	{
		public Plan Plan { get; set; }
		public Subscription Subscription { get; set; }
		public IdentityUser IdentityUser { get; set; }
	}
}
