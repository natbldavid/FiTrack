using FiTrack.Api.Models.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> entity)
    {
        entity.ToTable("tbl_activity_logs", t =>
        {
            t.HasCheckConstraint(
                "CK_tbl_activity_logs_duration",
                "[duration_minutes] > 0");
        });

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        entity.Property(x => x.ActivityTypeId)
            .HasColumnName("activity_type_id")
            .IsRequired();

        entity.Property(x => x.LogDate)
            .HasColumnName("log_date")
            .HasColumnType("date")
            .IsRequired();

        entity.Property(x => x.DurationMinutes)
            .HasColumnName("duration_minutes")
            .IsRequired();

        entity.Property(x => x.Distance)
            .HasColumnName("distance")
            .HasPrecision(8, 2);

        entity.Property(x => x.CaloriesBurned)
            .HasColumnName("calories_burned");

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

        entity.HasIndex(x => new { x.UserId, x.LogDate });

        entity.HasIndex(x => x.ActivityTypeId);

        entity.HasOne(x => x.User)
            .WithMany(x => x.ActivityLogs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.ActivityType)
            .WithMany(x => x.ActivityLogs)
            .HasForeignKey(x => x.ActivityTypeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}