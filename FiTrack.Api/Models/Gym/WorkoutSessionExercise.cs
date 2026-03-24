namespace FiTrack.Api.Models.Gym;

public class WorkoutSessionExercise
{
    public int Id { get; set; }

    public int WorkoutSessionId { get; set; }

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

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public WorkoutSession WorkoutSession { get; set; } = null!;
    public ExerciseCatalog ExerciseCatalog { get; set; } = null!;

    public ICollection<WorkoutSetLog> WorkoutSetLogs { get; set; } = new List<WorkoutSetLog>();
}