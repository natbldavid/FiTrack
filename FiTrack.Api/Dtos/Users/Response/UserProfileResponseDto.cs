namespace FiTrack.Api.Dtos.Users.Responses;

public class UserProfileResponseDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;

    public decimal? CurrentWeight { get; set; }

    public int? DailyCalorieGoal { get; set; }
    public int? DailyProteinGoal { get; set; }
    public int? DailyCarbGoal { get; set; }
    public int? DailyFatGoal { get; set; }

    public decimal? WeightGoal { get; set; }

    public int? WeeklyExerciseGoal { get; set; }

    public int? WeeklyGymGoal { get; set; }

    public DateTime UpdatedAt { get; set; }
}