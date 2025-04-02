using EventFlow_API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlow_API.Data.Mapping;

public class EventMap : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Event");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnName("Title")
            .HasColumnType("VARCHAR")
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasColumnType("VARCHAR")
            .HasMaxLength(8000);

        builder.Property(x => x.Date)
            .IsRequired()
            .HasColumnName("Date")
            .HasColumnType("DATETIME");

        builder.Property(x => x.Location)
            .IsRequired()
            .HasColumnName("Location")
            .HasColumnType("VARCHAR");

        builder.Property(x => x.OrganizerId)
            .IsRequired()
            .HasColumnName("OrganizerId")
            .HasColumnType("INT");

        builder
            .HasMany(e => e.Speakers)
            .WithOne(s => s.Event)
            .HasForeignKey(s => s.EventId);

        builder
            .HasOne(e => e.Organizer)
            .WithMany(o => o.Events)
            .HasForeignKey(e => e.OrganizerId);

        builder
            .HasMany(e => e.Participants)
            .WithMany(p => p.Events)
            .UsingEntity("EventParticipant");
    }
}


