using Microsoft.EntityFrameworkCore;
using SecretariaIa.Api.Models;

namespace SecretariaIa.Api
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<InboundMessage> InboundMessages => Set<InboundMessage>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<InboundMessage>(e =>
			{
				e.ToTable("InboundMessages");
				e.HasKey(x => x.Id);
				e.Property(x => x.From).HasMaxLength(50).IsRequired();
				e.Property(x => x.To).HasMaxLength(50).IsRequired();
				e.Property(x => x.Body).HasMaxLength(4000).IsRequired();
				e.Property(x => x.MessageSid).HasMaxLength(80);
			});
		}
	}
}
