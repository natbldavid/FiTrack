using FiTrack.Api.Models.Gym;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class WorkoutDayExerciseConfiguration : IEntityTypeConfiguration<WorkoutDayExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutDayExercise> entity)
    {
        entity.ToTable("tbl_workout_day_exercises");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(x => x.WorkoutDayId)
            .HasColumnName("workout_day_id")
            .IsRequired();

        entity.Property(x => x.ExerciseId)
            .HasColumnName("exercise_id")
            .IsRequired();

        entity.Property(x => x.ExerciseOrder)
            .HasColumnName("exercise_order")
            .IsRequired();

        entity.Property(x => x.TargetSets)
            .HasColumnName("target_sets")
            .IsRequired();

        entity.Property(x => x.TargetRepsMin)
            .HasColumnName("target_reps_min")
            .IsRequired();

        entity.Property(x => x.TargetRepsMax)
            .HasColumnName("target_reps_max")
            .IsRequired();

        entity.Property(x => x.InitialWeight)
            .HasColumnName("initial_weight")
            .HasPrecision(6, 2);

        entity.Property(x => x.CurrentWorkingWeight)
            .HasColumnName("current_working_weight")
            .HasPrecision(6, 2);

        entity.Property(x => x.Notes)
            .HasColumnName("notes")
            .HasColumnType("nvarchar(max)");

        entity.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.HasIndex(x => x.WorkoutDayId);

        entity.HasIndex(x => x.ExerciseId);

        entity.HasIndex(x => new { x.WorkoutDayId, x.ExerciseOrder })
            .IsUnique();

        entity.HasOne(x => x.WorkoutDay)
            .WithMany(x => x.WorkoutDayExercises)
            .HasForeignKey(x => x.WorkoutDayId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.ExerciseCatalog)
            .WithMany(x => x.WorkoutDayExercises)
            .HasForeignKey(x => x.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_tbl_workout_day_exercises_target_sets",
                "[target_sets] > 0");

            t.HasCheckConstraint(
                "CK_tbl_workout_day_exercises_target_reps_min",
                "[target_reps_min] > 0");

            t.HasCheckConstraint(
                "CK_tbl_workout_day_exercises_target_reps_max",
                "[target_reps_max] > 0");

            t.HasCheckConstraint(
                "CK_tbl_workout_day_exercises_target_reps_range",
                "[target_reps_max] >= [target_reps_min]");

            t.HasCheckConstraint(
                "CK_tbl_workout_day_exercises_exercise_order",
                "[exercise_order] > 0");
        });
    }
}