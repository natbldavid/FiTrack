using FiTrack.Api.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiTrack.Api.Data.Configurations;

public class WeightLogConfiguration : IEntityTypeConfiguration<WeightLog>
{
    public void Configure(EntityTypeBuilder<WeightLog> entity)
    {
        entity.ToTable("tbl_weight_logs");

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

        entity.Property(x => x.Weight)
            .HasColumnName("weight")
            .HasPrecision(5, 2)
            .IsRequired();

        entity.Property(x => x.Note)
            .HasColumnName("note")
            .HasColumnType("text");

        entity.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime2")
            .IsRequired();

        entity.HasIndex(x => new { x.UserId, x.LogDate })
            .IsUnique();

        entity.HasOne(x => x.User)
            .WithMany(x => x.WeightLogs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}