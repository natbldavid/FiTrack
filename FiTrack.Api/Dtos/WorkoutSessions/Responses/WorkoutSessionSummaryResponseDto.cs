namespace FiTrack.Api.Dtos.WorkoutSessions.Responses;

public class WorkoutSessionSummaryResponseDto
{
    public int Id { get; set; }
    public string SessionName { get; set; } = null!;
    public DateOnly SessionDate { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Status { get; set; } = null!;
    public int ExerciseCount { get; set; }
}