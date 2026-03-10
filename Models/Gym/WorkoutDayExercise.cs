namespace FiTrack.Api.Models.Gym;

public class WorkoutDayExercise
{
    public int Id { get; set; }

    public int WorkoutDayId { get; set; }

    public int ExerciseId { get; set; }

    public int ExerciseOrder { get; set; }

    public int TargetSets { get; set; }

    public int TargetRepsMin { get; set; }

    public int TargetRepsMax { get; set; }

    public decimal? InitialWeight { get; set; }

    public decimal? CurrentWorkingWeight { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public WorkoutDay WorkoutDay { get; set; } = null!;
    public ExerciseCatalog ExerciseCatalog { get; set; } = null!;
}