namespace FiTrack.Api.Models.Users;

using FiTrack.Api.Models.Activities;
using FiTrack.Api.Models.Food;
using FiTrack.Api.Models.Gym;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasscodeHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public UserProfile? Profile { get; set; }

    public ICollection<WeightLog> WeightLogs { get; set; } = new List<WeightLog>();

    public ICollection<UserGoalsHistory> GoalsHistory { get; set; } = new List<UserGoalsHistory>();

    public ICollection<Food> Foods { get; set; } = new List<Food>();

    public ICollection<Meal> Meals { get; set; } = new List<Meal>();

    public ICollection<FoodLog> FoodLogs { get; set; } = new List<FoodLog>();

    public ICollection<WorkoutDay> WorkoutDays { get; set; } = new List<WorkoutDay>();

    public ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();

    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
}