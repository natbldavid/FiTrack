using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.Users.Requests;

public class UpdateUserProfileRequestDto
{
    [Range(1, 1000)]
    public decimal? CurrentWeight { get; set; }

    [Range(1, 20000)]
    public int? DailyCalorieGoal { get; set; }

    [Range(1, 1000)]
    public int? DailyProteinGoal { get; set; }

    [Range(1, 1000)]
    public int? DailyCarbGoal { get; set; }

    [Range(1, 1000)]
    public int? DailyFatGoal { get; set; }

    [Range(1, 1000)]
    public decimal? WeightGoal { get; set; }

    [Range(0, 10000)]
    public int? WeeklyExerciseGoal { get; set; }

    [Range(0, 14)]
    public int? WeeklyGymGoal { get; set; }
}