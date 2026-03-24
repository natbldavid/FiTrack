namespace FiTrack.Api.Dtos.WorkoutSessions.Responses;

public class WorkoutSessionExerciseResponseDto
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }

    public string ExerciseNameSnapshot { get; set; } = null!;
    public string? BodyPartSnapshot { get; set; }
    public string? ExerciseTypeSnapshot { get; set; }

    public int ExerciseOrder { get; set; }

    public int? TargetSets { get; set; }
    public int? TargetRepsMin { get; set; }
    public int? TargetRepsMax { get; set; }

    public decimal? PlannedWorkingWeight { get; set; }
    public decimal? ActualWorkingWeight { get; set; }

    public string? Notes { get; set; }

    public List<WorkoutSetLogResponseDto> Sets { get; set; } = new();
}