using FiTrack.Api.Models.Gym;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class ExerciseCatalogConfiguration : IEntityTypeConfiguration<ExerciseCatalog>
{
    public void Configure(EntityTypeBuilder<ExerciseCatalog> entity)
    {
        entity.ToTable("lkp_exercise_catalog", t =>
        {
            t.HasCheckConstraint(
                "CK_lkp_exercise_catalog_exercise_type",
                "[exercise_type] IN ('compound', 'accessory')");

            t.HasCheckConstraint(
                "CK_lkp_exercise_catalog_body_part",
                "[body_part] IS NULL OR [body_part] IN ('chest', 'shoulders', 'bicep', 'tricep', 'back', 'legs', 'core')");
        });

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(x => x.BodyPart)
            .HasColumnName("body_part")
            .HasMaxLength(50)
            .IsRequired();

        entity.Property(x => x.ExerciseType)
            .HasColumnName("exercise_type")
            .HasMaxLength(20)
            .IsRequired();

        entity.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        entity.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.HasIndex(x => x.Name)
            .IsUnique();

        entity.HasIndex(x => x.BodyPart);

        entity.HasIndex(x => x.ExerciseType);

        entity.HasIndex(x => x.IsActive);
    }
}