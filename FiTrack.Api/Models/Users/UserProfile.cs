namespace FiTrack.Api.Models.Users;

public class UserProfile
{
    public int UserId { get; set; }

    public decimal? CurrentWeight { get; set; }

    public int? DailyCalorieGoal { get; set; }
    public int? DailyProteinGoal { get; set; }
    public int? DailyCarbGoal { get; set; }
    public int? DailyFatGoal { get; set; }

    public decimal? WeightGoal { get; set; }

    public int? WeeklyExerciseGoal { get; set; }
    public int? WeeklyGymGoal { get; set; }

    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
}