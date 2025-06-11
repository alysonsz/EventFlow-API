namespace EventFlow.Infrastructure.Data.Mapping;

public class SpeakerEventMap : IEntityTypeConfiguration<SpeakerEvent>
{
    public void Configure(EntityTypeBuilder<SpeakerEvent> builder)
    {
        builder.ToTable("SpeakerEvent");

        builder.HasKey(se => new { se.SpeakerId, se.EventId });

        builder.Property(se => se.RegisteredAt)
            .IsRequired()
            .HasColumnType("DATETIME");

        builder.HasOne(se => se.Speaker)
            .WithMany(s => s.SpeakerEvents)
            .HasForeignKey(se => se.SpeakerId);

        builder.HasOne(se => se.Event)
            .WithMany(e => e.SpeakerEvents)
            .HasForeignKey(se => se.EventId);
    }
}
