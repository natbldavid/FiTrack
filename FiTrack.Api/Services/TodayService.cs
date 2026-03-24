using FiTrack.Api.Data;
using FiTrack.Api.Dtos.ActivityLogs.Responses;
using FiTrack.Api.Dtos.FoodLogs.Responses;
using FiTrack.Api.Dtos.Today.Responses;
using FiTrack.Api.Dtos.WorkoutSessions.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class TodayService : ITodayService
{
    private readonly FiTrackDbContext _context;

    public TodayService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<TodayDashboardResponseDto> GetTodayDashboardAsync(int userId, DateOnly date)
    {
        var weekStart = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
        if (date.DayOfWeek == DayOfWeek.Sunday)
        {
            weekStart = date.AddDays(-6);
        }

        var weekEnd = weekStart.AddDays(6);

        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var foodLogs = await _context.FoodLogs
            .Where(fl => fl.UserId == userId && fl.LogDate == date)
            .OrderBy(fl => fl.LoggedAt)
            .Select(fl => new FoodLogResponseDto
            {
                Id = fl.Id,
                LogDate = fl.LogDate,
                LoggedAt = fl.LoggedAt,
                SourceType = fl.SourceType,
                FoodId = fl.FoodId,
                MealId = fl.MealId,
                NameSnapshot = fl.NameSnapshot,
                Calories = fl.Calories,
                Protein = fl.Protein,
                Carbs = fl.Carbs,
                Fat = fl.Fat,
                Quantity = fl.Quantity,
                MealSlot = fl.MealSlot,
                Note = fl.Note
            })
            .ToListAsync();

        var activityLogs = await _context.Set<FiTrack.Api.Models.Activities.ActivityLog>()
            .Where(al => al.UserId == userId && al.LogDate == date)
            .Include(al => al.ActivityType)
            .OrderBy(al => al.Id)
            .Select(al => new ActivityLogResponseDto
            {
                Id = al.Id,
                ActivityTypeId = al.ActivityTypeId,
                ActivityTypeName = al.ActivityType.Name,
                ActivityTypeIcon = al.ActivityType.Icon,
                LogDate = al.LogDate,
                DurationMinutes = al.DurationMinutes,
                Distance = al.Distance,
                CaloriesBurned = al.CaloriesBurned,
                Notes = al.Notes
            })
            .ToListAsync();

        var todayWeight = await _context.WeightLogs
            .Where(w => w.UserId == userId && w.LogDate == date)
            .OrderByDescending(w => w.LoggedAt)
            .Select(w => (decimal?)w.Weight)
            .FirstOrDefaultAsync();

        var weeklyExerciseMinutes = await _context.Set<FiTrack.Api.Models.Activities.ActivityLog>()
            .Where(al => al.UserId == userId &&
                         al.LogDate >= weekStart &&
                         al.LogDate <= weekEnd)
            .SumAsync(al => al.DurationMinutes);

        var activeWorkoutEntity = await _context.WorkoutSessions
            .Include(ws => ws.WorkoutSessionExercises)
            .Where(ws => ws.UserId == userId && ws.Status == "in_progress")
            .OrderByDescending(ws => ws.StartedAt)
            .FirstOrDefaultAsync();

        var completedWorkoutTodayEntity = await _context.WorkoutSessions
            .Include(ws => ws.WorkoutSessionExercises)
            .Where(ws => ws.UserId == userId &&
                         ws.SessionDate == date &&
                         ws.Status == "completed")
            .OrderByDescending(ws => ws.CompletedAt)
            .FirstOrDefaultAsync();

        var plannedWorkoutEntity = await _context.WorkoutDays
            .Where(wd => wd.UserId == userId)
            .OrderBy(wd => wd.SortOrder)
            .ThenBy(wd => wd.Name)
            .FirstOrDefaultAsync();

        var caloriesConsumed = foodLogs.Sum(x => x.Calories);
        var proteinConsumed = foodLogs.Sum(x => x.Protein);
        var carbsConsumed = foodLogs.Sum(x => x.Carbs);
        var fatConsumed = foodLogs.Sum(x => x.Fat);

        var dailyCalorieGoal = user.Profile?.DailyCalorieGoal ?? 0;
        var proteinGoal = user.Profile?.DailyProteinGoal ?? 0;
        var carbsGoal = user.Profile?.DailyCarbGoal ?? 0;
        var fatGoal = user.Profile?.DailyFatGoal ?? 0;

        return new TodayDashboardResponseDto
        {
            Date = date,

            DailyCalorieGoal = dailyCalorieGoal,
            CaloriesConsumed = caloriesConsumed,
            CaloriesRemaining = dailyCalorieGoal - caloriesConsumed,

            ProteinConsumed = proteinConsumed,
            ProteinGoal = proteinGoal,

            CarbsConsumed = carbsConsumed,
            CarbsGoal = carbsGoal,

            FatConsumed = fatConsumed,
            FatGoal = fatGoal,

            TodayWeight = todayWeight,
            WeightGoal = user.Profile?.WeightGoal,

            WeeklyExerciseMinutes = weeklyExerciseMinutes,
            WeeklyExerciseGoal = user.Profile?.WeeklyExerciseGoal ?? 0,

            PlannedWorkoutName = plannedWorkoutEntity?.Name,
            ActiveWorkout = activeWorkoutEntity is null ? null : MapWorkoutSessionSummary(activeWorkoutEntity),
            CompletedWorkoutToday = completedWorkoutTodayEntity is null ? null : MapWorkoutSessionSummary(completedWorkoutTodayEntity),

            FoodLogs = foodLogs,
            ActivityLogs = activityLogs
        };
    }

    private static WorkoutSessionSummaryResponseDto MapWorkoutSessionSummary(FiTrack.Api.Models.Gym.WorkoutSession session)
    {
        return new WorkoutSessionSummaryResponseDto
        {
            Id = session.Id,
            SessionName = session.SessionName,
            SessionDate = session.SessionDate,
            StartedAt = session.StartedAt,
            CompletedAt = session.CompletedAt,
            Status = session.Status,
            ExerciseCount = session.WorkoutSessionExercises.Count
        };
    }
}