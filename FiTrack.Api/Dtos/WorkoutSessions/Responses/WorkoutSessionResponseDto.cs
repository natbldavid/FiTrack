namespace FiTrack.Api.Dtos.WorkoutSessions.Responses;

public class WorkoutSessionResponseDto
{
    public int Id { get; set; }
    public int? WorkoutDayId { get; set; }

    public string SessionName { get; set; } = null!;
    public DateOnly SessionDate { get; set; }

    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public string Status { get; set; } = null!;
    public string? Notes { get; set; }

    public List<WorkoutSessionExerciseResponseDto> Exercises { get; set; } = new();
}