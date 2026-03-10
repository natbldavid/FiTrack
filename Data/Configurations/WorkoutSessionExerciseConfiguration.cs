using FiTrack.Api.Models.Gym;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class WorkoutSessionExerciseConfiguration : IEntityTypeConfiguration<WorkoutSessionExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutSessionExercise> entity)
    {
        entity.ToTable("tbl_workout_session_exercises", t =>
        {
            t.HasCheckConstraint(
                "CK_tbl_workout_session_exercises_exercise_order",
                "[exercise_order] > 0");

            t.HasCheckConstraint(
                "CK_tbl_workout_session_exercises_target_sets",
                "[target_sets] IS NULL OR [target_sets] > 0");

            t.HasCheckConstraint(
                "CK_tbl_workout_session_exercises_target_reps_min",
                "[target_reps_min] IS NULL OR [target_reps_min] > 0");

            t.HasCheckConstraint(
                "CK_tbl_workout_session_exercises_target_reps_max",
                "[target_reps_max] IS NULL OR [target_reps_max] > 0");

            t.HasCheckConstraint(
                "CK_tbl_workout_session_exercises_target_reps_range",
                "([target_reps_min] IS NULL AND [target_reps_max] IS NULL) OR " +
                "([target_reps_min] IS NOT NULL AND [target_reps_max] IS NOT NULL AND [target_reps_max] >= [target_reps_min])");
        });

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(x => x.WorkoutSessionId)
            .HasColumnName("workout_session_id")
            .IsRequired();

        entity.Property(x => x.ExerciseId)
            .HasColumnName("exercise_id")
            .IsRequired();

        entity.Property(x => x.ExerciseNameSnapshot)
            .HasColumnName("exercise_name_snapshot")
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(x => x.BodyPartSnapshot)
            .HasColumnName("body_part_snapshot")
            .HasMaxLength(50);

        entity.Property(x => x.ExerciseTypeSnapshot)
            .HasColumnName("exercise_type_snapshot")
            .HasMaxLength(20);

        entity.Property(x => x.ExerciseOrder)
            .HasColumnName("exercise_order")
            .IsRequired();

        entity.Property(x => x.TargetSets)
            .HasColumnName("target_sets");

        entity.Property(x => x.TargetRepsMin)
            .HasColumnName("target_reps_min");

        entity.Property(x => x.TargetRepsMax)
            .HasColumnName("target_reps_max");

        entity.Property(x => x.PlannedWorkingWeight)
            .HasColumnName("planned_working_weight")
            .HasPrecision(6, 2);

        entity.Property(x => x.ActualWorkingWeight)
            .HasColumnName("actual_working_weight")
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

        entity.HasIndex(x => x.WorkoutSessionId);

        entity.HasIndex(x => x.ExerciseId);

        entity.HasIndex(x => new { x.WorkoutSessionId, x.ExerciseOrder })
            .IsUnique();

        entity.HasOne(x => x.WorkoutSession)
            .WithMany(x => x.WorkoutSessionExercises)
            .HasForeignKey(x => x.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.ExerciseCatalog)
            .WithMany(x => x.WorkoutSessionExercises)
            .HasForeignKey(x => x.ExerciseId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}