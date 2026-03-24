namespace FiTrack.Api.Dtos.WorkoutDays.Responses;

public class WorkoutDayExerciseResponseDto
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public string ExerciseName { get; set; } = null!;
    public string BodyPart { get; set; } = null!;
    public string ExerciseType { get; set; } = null!;

    public int ExerciseOrder { get; set; }

    public int TargetSets { get; set; }
    public int TargetRepsMin { get; set; }
    public int TargetRepsMax { get; set; }

    public decimal? InitialWeight { get; set; }
    public decimal? CurrentWorkingWeight { get; set; }

    public string? Notes { get; set; }
}