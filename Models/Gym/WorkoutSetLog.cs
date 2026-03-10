namespace FiTrack.Api.Models.Gym;

public class WorkoutSetLog
{
    public int Id { get; set; }

    public int WorkoutSessionExerciseId { get; set; }

    public int SetNumber { get; set; }

    public int Reps { get; set; }

    public decimal? Weight { get; set; }

    public bool Completed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public WorkoutSessionExercise WorkoutSessionExercise { get; set; } = null!;
}