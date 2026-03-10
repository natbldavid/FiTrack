namespace FiTrack.Api.Models.Activities;

public class ActivityLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ActivityTypeId { get; set; }

    public DateOnly LogDate { get; set; }

    public int DurationMinutes { get; set; }

    public decimal? Distance { get; set; }

    public int? CaloriesBurned { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Users.User User { get; set; } = null!;

    public ActivityType ActivityType { get; set; } = null!;
}