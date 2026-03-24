namespace FiTrack.Api.Models.Gym;

public class WorkoutDay
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? MuscleFocus { get; set; }

    public int? SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Users.User User { get; set; } = null!;

    public ICollection<WorkoutDayExercise> WorkoutDayExercises { get; set; } = new List<WorkoutDayExercise>();

    public ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
}