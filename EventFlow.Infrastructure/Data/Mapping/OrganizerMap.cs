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

        builder.OwnsOne(x => x.Name, name =>
        {
            name.Property(n => n.FirstName)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR")
                .HasMaxLength(200);
        });

        builder.OwnsOne(x => x.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .HasColumnType("VARCHAR")
                .HasMaxLength(150);
        });

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder
            .HasMany(o => o.Events)
            .WithOne(e => e.Organizer)
            .HasForeignKey(e => e.OrganizerId)
            .HasConstraintName("FK_Event_Organizer");
    }
}

