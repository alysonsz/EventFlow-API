using EventFlow_API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlow_API.Data.Mapping;

public class ParticipantMap : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("Participant");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("VARCHAR")
            .HasMaxLength(200);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(150);

        builder
            .HasMany(p => p.Events)
            .WithMany(e => e.Participants)
            .UsingEntity("EventParticipant");
    }
}

