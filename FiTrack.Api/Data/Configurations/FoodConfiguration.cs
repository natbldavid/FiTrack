using FiTrack.Api.Models.Food;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> entity)
    {
        entity.ToTable("tbl_foods");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        entity.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(x => x.ServingDescription)
            .HasColumnName("serving_description")
            .HasMaxLength(200);

        entity.Property(x => x.Calories)
            .HasColumnName("calories")
            .HasDefaultValue(0)
            .IsRequired();

        entity.Property(x => x.Protein)
            .HasColumnName("protein")
            .HasPrecision(6, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        entity.Property(x => x.Carbs)
            .HasColumnName("carbs")
            .HasPrecision(6, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        entity.Property(x => x.Fat)
            .HasColumnName("fat")
            .HasPrecision(6, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        entity.Property(x => x.IsFavorite)
            .HasColumnName("is_favorite")
            .HasDefaultValue(false)
            .IsRequired();

        entity.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.HasIndex(x => x.UserId);

        entity.HasIndex(x => new { x.UserId, x.Name });

        entity.HasOne(x => x.User)
            .WithMany(x => x.Foods)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}