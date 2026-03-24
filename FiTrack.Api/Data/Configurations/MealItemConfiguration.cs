using FiTrack.Api.Models.Food;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class MealItemConfiguration : IEntityTypeConfiguration<MealItem>
{
    public void Configure(EntityTypeBuilder<MealItem> entity)
    {
        entity.ToTable("tbl_meal_items");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(x => x.MealId)
            .HasColumnName("meal_id")
            .IsRequired();

        entity.Property(x => x.FoodId)
            .HasColumnName("food_id")
            .IsRequired();

        entity.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .HasPrecision(8, 2)
            .HasDefaultValue(1m)
            .IsRequired();

        entity.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.HasIndex(x => x.MealId);

        entity.HasIndex(x => x.FoodId);

        entity.HasIndex(x => new { x.MealId, x.FoodId })
        .IsUnique();

        entity.HasOne(x => x.Meal)
            .WithMany(x => x.MealItems)
            .HasForeignKey(x => x.MealId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.Food)
            .WithMany(x => x.MealItems)
            .HasForeignKey(x => x.FoodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}