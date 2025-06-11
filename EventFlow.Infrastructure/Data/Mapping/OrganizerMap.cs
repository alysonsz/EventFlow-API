namespace EventFlow.Infrastructure.Data.Mapping;

public class OrganizerMap : IEntityTypeConfiguration<Organizer>
{
    public void Configure(EntityTypeBuilder<Organizer> builder)
    {
        builder.ToTable("Organizer");

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
            .HasMany(o => o.Events)
            .WithOne(e => e.Organizer)
            .HasForeignKey(e => e.OrganizerId)
            .HasConstraintName("FK_Event_Organizer");
    }
}

