using FiTrack.Api.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("tbl_users");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(x => x.Username)
            .HasColumnName("username")
            .HasMaxLength(50)
            .IsRequired();

        entity.HasIndex(x => x.Username)
            .IsUnique();

        entity.Property(x => x.PasscodeHash)
            .HasColumnName("passcode_hash")
            .HasMaxLength(255)
            .IsRequired();

        entity.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.HasData(
            new User
            {
                Id = 1,
                Username = "demo_user",
                PasscodeHash = "AQAAAAIAAYagAAAAEMOCK8GkM8J7W0X0dM5mA3O3Nn7Q3qV2Pq1r0J9G4k8x6mVh2Qq8qP1p4L5j9A==",
                CreatedAt = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}