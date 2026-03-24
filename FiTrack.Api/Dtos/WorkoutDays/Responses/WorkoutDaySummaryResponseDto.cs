namespace FiTrack.Api.Dtos.WorkoutDays.Responses;

public class WorkoutDaySummaryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? MuscleFocus { get; set; }
    public int? SortOrder { get; set; }

    public int ExerciseCount { get; set; }
    public DateOnly? LastCompletedDate { get; set; }
}