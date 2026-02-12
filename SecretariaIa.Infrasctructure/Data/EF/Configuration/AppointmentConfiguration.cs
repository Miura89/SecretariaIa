using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretariaIa.Domain.Entities;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
	public void Configure(EntityTypeBuilder<Appointment> builder)
	{
		builder.ToTable("Appointments");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Title)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.ScheduledAt)
			.IsRequired();

		builder.Property(x => x.RemindBeforeMinutes)
			.IsRequired();

		builder.Property(x => x.ReminderSent)
			.IsRequired();

		builder.Property(x => x.CreatedAt)
			.IsRequired();

		builder.HasOne(x => x.IdentityUser)
			.WithMany()
			.HasForeignKey(x => x.IdentityUserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
