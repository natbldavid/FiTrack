namespace FiTrack.Api.Dtos.ActivityLogs.Responses;

public class ActivityLogResponseDto
{
    public int Id { get; set; }
    public int ActivityTypeId { get; set; }
    public string ActivityTypeName { get; set; } = null!;
    public string ActivityTypeIcon { get; set; } = null!;

    public DateOnly LogDate { get; set; }
    public int DurationMinutes { get; set; }
    public decimal? Distance { get; set; }
    public int? CaloriesBurned { get; set; }

    public string? Notes { get; set; }
}