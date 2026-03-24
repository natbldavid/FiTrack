namespace FiTrack.Api.Dtos.Users.Responses;

public class UserGoalsHistoryResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }

    public int? DailyCalorieGoal { get; set; }
    public int? DailyProteinGoal { get; set; }
    public int? DailyCarbGoal { get; set; }
    public int? DailyFatGoal { get; set; }

    public decimal? WeightGoal { get; set; }

    public int? WeeklyExerciseGoal { get; set; }

    public DateTime CreatedAt { get; set; }
}