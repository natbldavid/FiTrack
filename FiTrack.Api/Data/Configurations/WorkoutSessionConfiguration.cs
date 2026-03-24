using FiTrack.Api.Models.Gym;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> entity)
    {
        entity.ToTable("tbl_workout_sessions", t =>
        {
            t.HasCheckConstraint(
                "CK_tbl_workout_sessions_status",
                "[status] IN ('in_progress', 'completed', 'cancelled')");
        });

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        entity.Property(x => x.WorkoutDayId)
            .HasColumnName("workout_day_id");

        entity.Property(x => x.SessionName)
            .HasColumnName("session_name")
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(x => x.SessionDate)
            .HasColumnName("session_date")
            .HasColumnType("date")
            .IsRequired();

        entity.Property(x => x.StartedAt)
            .HasColumnName("started_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.Property(x => x.CompletedAt)
            .HasColumnName("completed_at")
            .HasColumnType("datetime2");

        entity.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .IsRequired();

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

        entity.HasIndex(x => x.UserId);

        entity.HasIndex(x => new { x.UserId, x.SessionDate });

        entity.HasIndex(x => new { x.UserId, x.StartedAt });

        entity.HasIndex(x => x.WorkoutDayId);

        entity.HasIndex(x => x.Status);

        entity.HasOne(x => x.User)
            .WithMany(x => x.WorkoutSessions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.WorkoutDay)
            .WithMany(x => x.WorkoutSessions)
            .HasForeignKey(x => x.WorkoutDayId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}