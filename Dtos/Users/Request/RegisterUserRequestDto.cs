using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.Users.Requests;

public class RegisterUserRequestDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Passcode { get; set; } = null!;

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
}