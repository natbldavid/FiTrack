namespace FiTrack.Api.Models.Gym;

public class ExerciseCatalog
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string BodyPart { get; set; } = null!;

    public string ExerciseType { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<WorkoutDayExercise> WorkoutDayExercises { get; set; } = new List<WorkoutDayExercise>();

    public ICollection<WorkoutSessionExercise> WorkoutSessionExercises { get; set; } = new List<WorkoutSessionExercise>();
}