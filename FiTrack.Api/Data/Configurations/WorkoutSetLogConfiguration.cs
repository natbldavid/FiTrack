using FiTrack.Api.Models.Gym;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class WorkoutSetLogConfiguration : IEntityTypeConfiguration<WorkoutSetLog>
{
    public void Configure(EntityTypeBuilder<WorkoutSetLog> entity)
    {
        entity.ToTable("tbl_workout_set_logs", t =>
        {
            t.HasCheckConstraint(
                "CK_tbl_workout_set_logs_set_number",
                "[set_number] > 0");

            t.HasCheckConstraint(
                "CK_tbl_workout_set_logs_reps",
                "[reps] >= 0");
        });

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(x => x.WorkoutSessionExerciseId)
            .HasColumnName("workout_session_exercise_id")
            .IsRequired();

        entity.Property(x => x.SetNumber)
            .HasColumnName("set_number")
            .IsRequired();

        entity.Property(x => x.Reps)
            .HasColumnName("reps")
            .IsRequired();

        entity.Property(x => x.Weight)
            .HasColumnName("weight")
            .HasPrecision(6, 2);

        entity.Property(x => x.Completed)
            .HasColumnName("completed")
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

        entity.HasIndex(x => x.WorkoutSessionExerciseId);

        entity.HasIndex(x => new { x.WorkoutSessionExerciseId, x.SetNumber })
            .IsUnique();

        entity.HasOne(x => x.WorkoutSessionExercise)
            .WithMany(x => x.WorkoutSetLogs)
            .HasForeignKey(x => x.WorkoutSessionExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}