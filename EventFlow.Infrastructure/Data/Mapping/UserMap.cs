using EventFlow.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlow.Infrastructure.Data.Mapping;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Username)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(150);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasColumnType("VARCHAR(MAX)");

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()"); 
    }
}