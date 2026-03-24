namespace FiTrack.Api.Dtos.WorkoutDays.Responses;

public class WorkoutDayDetailResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? MuscleFocus { get; set; }
    public int? SortOrder { get; set; }

    public List<WorkoutDayExerciseResponseDto> Exercises { get; set; } = new();
}