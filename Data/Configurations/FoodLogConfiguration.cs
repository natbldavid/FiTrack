using FiTrack.Api.Models.Food;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class FoodLogConfiguration : IEntityTypeConfiguration<FoodLog>
{
    public void Configure(EntityTypeBuilder<FoodLog> entity)
    {
        entity.ToTable("tbl_food_logs");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        entity.Property(x => x.LogDate)
            .HasColumnName("log_date")
            .HasColumnType("date")
            .IsRequired();

        entity.Property(x => x.LoggedAt)
            .HasColumnName("logged_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.Property(x => x.SourceType)
            .HasColumnName("source_type")
            .HasMaxLength(20)
            .IsRequired();

        entity.Property(x => x.FoodId)
            .HasColumnName("food_id");

        entity.Property(x => x.MealId)
            .HasColumnName("meal_id");

        entity.Property(x => x.NameSnapshot)
            .HasColumnName("name_snapshot")
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(x => x.Calories)
            .HasColumnName("calories")
            .IsRequired();

        entity.Property(x => x.Protein)
            .HasColumnName("protein")
            .HasPrecision(6, 2)
            .IsRequired();

        entity.Property(x => x.Carbs)
            .HasColumnName("carbs")
            .HasPrecision(6, 2)
            .IsRequired();

        entity.Property(x => x.Fat)
            .HasColumnName("fat")
            .HasPrecision(6, 2)
            .IsRequired();

        entity.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .HasPrecision(8, 2)
            .HasDefaultValue(1m)
            .IsRequired();

        entity.Property(x => x.MealSlot)
            .HasColumnName("meal_slot")
            .HasMaxLength(20);

        entity.Property(x => x.Note)
            .HasColumnName("note")
            .HasColumnType("nvarchar(max)");

        entity.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.HasIndex(x => new { x.UserId, x.LogDate });

        entity.HasIndex(x => new { x.UserId, x.LoggedAt });

        entity.HasIndex(x => x.FoodId);

        entity.HasIndex(x => x.MealId);

        entity.HasOne(x => x.User)
            .WithMany(x => x.FoodLogs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.Food)
            .WithMany(x => x.FoodLogs)
            .HasForeignKey(x => x.FoodId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(x => x.Meal)
            .WithMany(x => x.FoodLogs)
            .HasForeignKey(x => x.MealId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.ToTable("tbl_food_logs", t =>
        {
            t.HasCheckConstraint(
                "CK_tbl_food_logs_source_type",
                "[source_type] IN ('food', 'meal', 'custom')");

            t.HasCheckConstraint(
                "CK_tbl_food_logs_meal_slot",
                "[meal_slot] IS NULL OR [meal_slot] IN ('breakfast', 'lunch', 'dinner', 'snack')");
        });
    }
}