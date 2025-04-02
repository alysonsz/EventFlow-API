using EventFlow_API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlow_API.Data.Mapping;

public class SpeakerMap : IEntityTypeConfiguration<Speaker>
{
    public void Configure(EntityTypeBuilder<Speaker> builder)
    {
        builder.ToTable("Speaker");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("VARCHAR")
            .HasMaxLength(200);

        builder.Property(x => x.Biography)
            .IsRequired()
            .HasColumnName("Biography")
            .HasColumnType("VARCHAR")
            .HasMaxLength(2000);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(150);

        builder.Property(x => x.EventId)
            .IsRequired()
            .HasColumnName("EventId")
            .HasColumnType("INT");

        builder
            .HasOne(s => s.Event)
            .WithMany(e => e.Speakers)
            .HasForeignKey(s => s.EventId);
    }
}
