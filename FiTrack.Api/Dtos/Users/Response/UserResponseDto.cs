namespace FiTrack.Api.Dtos.Users.Responses;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public UserProfileResponseDto? Profile { get; set; }
}

public class UserProfileDto
{
    public decimal? CurrentWeight { get; set; }

    public int? DailyCalorieGoal { get; set; }
    public int? DailyProteinGoal { get; set; }
    public int? DailyCarbGoal { get; set; }
    public int? DailyFatGoal { get; set; }

    public Decimal? WeightGoal { get; set; }

    public int? WeeklyExerciseGoal { get; set; }

    public DateTime UpdatedAt { get; set; }
}