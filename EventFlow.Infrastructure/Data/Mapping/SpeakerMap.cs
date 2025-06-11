namespace EventFlow.Infrastructure.Data.Mapping;

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

        builder
            .HasMany(s => s.SpeakerEvents)
            .WithOne(se => se.Speaker)
            .HasForeignKey(se => se.SpeakerId);

    }
}
