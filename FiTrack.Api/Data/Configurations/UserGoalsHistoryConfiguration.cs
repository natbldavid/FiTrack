using FiTrack.Api.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class UserGoalsHistoryConfiguration : IEntityTypeConfiguration<UserGoalsHistory>
{
    public void Configure(EntityTypeBuilder<UserGoalsHistory> entity)
    {
        entity.ToTable("tbl_user_goals_history");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        entity.Property(x => x.EffectiveFrom)
            .HasColumnName("effective_from")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.Property(x => x.EffectiveTo)
            .HasColumnName("effective_to")
            .HasColumnType("datetime2");

        entity.Property(x => x.DailyCalorieGoal)
            .HasColumnName("daily_calorie_goal");

        entity.Property(x => x.DailyProteinGoal)
            .HasColumnName("daily_protein_goal");

        entity.Property(x => x.DailyCarbGoal)
            .HasColumnName("daily_carb_goal");

        entity.Property(x => x.DailyFatGoal)
            .HasColumnName("daily_fat_goal");

        entity.Property(x => x.WeightGoal)
            .HasColumnName("weight_goal")
            .HasPrecision(5, 2);

        entity.Property(x => x.WeeklyExerciseGoal)
            .HasColumnName("weekly_exercise_goal");

        entity.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.HasIndex(x => x.UserId);

        entity.HasIndex(x => new { x.UserId, x.EffectiveFrom });

        entity.HasOne(x => x.User)
            .WithMany(x => x.GoalsHistory)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}