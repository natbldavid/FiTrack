namespace FiTrack.Api.Dtos.ActivityLogs.Responses;

public class WeeklyActivitySummaryResponseDto
{
    public int TotalMinutes { get; set; }
    public int WeeklyGoalMinutes { get; set; }

    public List<ActivityLogResponseDto> Activities { get; set; } = new();
}