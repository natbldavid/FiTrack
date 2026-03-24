using FiTrack.Api.Dtos.ActivityLogs.Responses;
using FiTrack.Api.Dtos.FoodLogs.Responses;
using FiTrack.Api.Dtos.WorkoutSessions.Responses;

namespace FiTrack.Api.Dtos.Today.Responses;

public class TodayDashboardResponseDto
{
    public DateOnly Date { get; set; }

    public int DailyCalorieGoal { get; set; }
    public int CaloriesConsumed { get; set; }
    public int CaloriesRemaining { get; set; }

    public decimal ProteinConsumed { get; set; }
    public decimal ProteinGoal { get; set; }

    public decimal CarbsConsumed { get; set; }
    public decimal CarbsGoal { get; set; }

    public decimal FatConsumed { get; set; }
    public decimal FatGoal { get; set; }

    public decimal? TodayWeight { get; set; }
    public decimal? WeightGoal { get; set; }

    public int WeeklyExerciseMinutes { get; set; }
    public int WeeklyExerciseGoal { get; set; }

    public string? PlannedWorkoutName { get; set; }
    public WorkoutSessionSummaryResponseDto? ActiveWorkout { get; set; }
    public WorkoutSessionSummaryResponseDto? CompletedWorkoutToday { get; set; }

    public List<FoodLogResponseDto> FoodLogs { get; set; } = new();
    public List<ActivityLogResponseDto> ActivityLogs { get; set; } = new();
}