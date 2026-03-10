namespace FiTrack.Api.Models.Gym;

public class WorkoutSession
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? WorkoutDayId { get; set; }

    public string SessionName { get; set; } = null!;

    public DateOnly SessionDate { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Users.User User { get; set; } = null!;
    public WorkoutDay? WorkoutDay { get; set; }

    public ICollection<WorkoutSessionExercise> WorkoutSessionExercises { get; set; } = new List<WorkoutSessionExercise>();
}