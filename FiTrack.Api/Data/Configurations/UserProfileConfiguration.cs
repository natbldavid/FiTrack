using FiTrack.Api.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> entity)
    {
        entity.ToTable("tbl_user_profile");

        entity.HasKey(x => x.UserId);

        entity.Property(x => x.UserId)
            .HasColumnName("user_id");

        entity.Property(x => x.CurrentWeight)
            .HasColumnName("current_weight")
            .HasPrecision(5, 2);

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

        entity.Property(x => x.WeeklyGymGoal)         
            .HasColumnName("weekly_gym_goal");

        entity.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.HasOne(x => x.User)
            .WithOne(x => x.Profile)
            .HasForeignKey<UserProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}