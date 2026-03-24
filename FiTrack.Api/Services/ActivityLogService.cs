using FiTrack.Api.Data;
using FiTrack.Api.Dtos.ActivityLogs.Requests;
using FiTrack.Api.Dtos.ActivityLogs.Responses;
using FiTrack.Api.Models.Activities;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class ActivityLogService : IActivityLogService
{
    private readonly FiTrackDbContext _context;

    public ActivityLogService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<ActivityLogResponseDto> CreateActivityLogAsync(int userId, CreateActivityLogRequestDto request)
    {
        var activityType = await _context.Set<ActivityType>()
            .FirstOrDefaultAsync(x => x.Id == request.ActivityTypeId);

        if (activityType is null)
        {
            throw new KeyNotFoundException("Activity type not found.");
        }

        var now = DateTime.UtcNow;

        var activityLog = new ActivityLog
        {
            UserId = userId,
            ActivityTypeId = request.ActivityTypeId,
            LogDate = request.LogDate,
            DurationMinutes = request.DurationMinutes,
            Distance = request.Distance,
            CaloriesBurned = request.CaloriesBurned,
            Notes = request.Notes,
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.Set<ActivityLog>().Add(activityLog);
        await _context.SaveChangesAsync();

        return new ActivityLogResponseDto
        {
            Id = activityLog.Id,
            ActivityTypeId = activityLog.ActivityTypeId,
            ActivityTypeName = activityType.Name,
            ActivityTypeIcon = activityType.Icon,
            LogDate = activityLog.LogDate,
            DurationMinutes = activityLog.DurationMinutes,
            Distance = activityLog.Distance,
            CaloriesBurned = activityLog.CaloriesBurned,
            Notes = activityLog.Notes
        };
    }

    public async Task<ActivityLogResponseDto?> UpdateActivityLogAsync(int userId, int activityLogId, UpdateActivityLogRequestDto request)
    {
        var activityLog = await _context.Set<ActivityLog>()
            .FirstOrDefaultAsync(x => x.Id == activityLogId && x.UserId == userId);

        if (activityLog is null)
        {
            return null;
        }

        var activityType = await _context.Set<ActivityType>()
            .FirstOrDefaultAsync(x => x.Id == request.ActivityTypeId);

        if (activityType is null)
        {
            throw new KeyNotFoundException("Activity type not found.");
        }

        activityLog.ActivityTypeId = request.ActivityTypeId;
        activityLog.LogDate = request.LogDate;
        activityLog.DurationMinutes = request.DurationMinutes;
        activityLog.Distance = request.Distance;
        activityLog.CaloriesBurned = request.CaloriesBurned;
        activityLog.Notes = request.Notes;
        activityLog.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new ActivityLogResponseDto
        {
            Id = activityLog.Id,
            ActivityTypeId = activityLog.ActivityTypeId,
            ActivityTypeName = activityType.Name,
            ActivityTypeIcon = activityType.Icon,
            LogDate = activityLog.LogDate,
            DurationMinutes = activityLog.DurationMinutes,
            Distance = activityLog.Distance,
            CaloriesBurned = activityLog.CaloriesBurned,
            Notes = activityLog.Notes
        };
    }

    public async Task<List<ActivityLogResponseDto>> GetActivityLogsAsync(int userId)
    {
        return await _context.Set<ActivityLog>()
            .Where(x => x.UserId == userId)
            .Include(x => x.ActivityType)
            .OrderByDescending(x => x.LogDate)
            .ThenByDescending(x => x.Id)
            .Select(x => new ActivityLogResponseDto
            {
                Id = x.Id,
                ActivityTypeId = x.ActivityTypeId,
                ActivityTypeName = x.ActivityType.Name,
                ActivityTypeIcon = x.ActivityType.Icon,
                LogDate = x.LogDate,
                DurationMinutes = x.DurationMinutes,
                Distance = x.Distance,
                CaloriesBurned = x.CaloriesBurned,
                Notes = x.Notes
            })
            .ToListAsync();
    }

    public async Task<WeeklyActivitySummaryResponseDto> GetWeeklySummaryAsync(int userId, DateOnly weekStartDate)
    {
        var weekEndDate = weekStartDate.AddDays(6);

        var activities = await _context.Set<ActivityLog>()
            .Where(x => x.UserId == userId &&
                        x.LogDate >= weekStartDate &&
                        x.LogDate <= weekEndDate)
            .Include(x => x.ActivityType)
            .OrderBy(x => x.LogDate)
            .ThenBy(x => x.Id)
            .Select(x => new ActivityLogResponseDto
            {
                Id = x.Id,
                ActivityTypeId = x.ActivityTypeId,
                ActivityTypeName = x.ActivityType.Name,
                ActivityTypeIcon = x.ActivityType.Icon,
                LogDate = x.LogDate,
                DurationMinutes = x.DurationMinutes,
                Distance = x.Distance,
                CaloriesBurned = x.CaloriesBurned,
                Notes = x.Notes
            })
            .ToListAsync();

        var weeklyGoalMinutes = 0;

        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user?.Profile?.WeeklyExerciseGoal is int weeklyGoal)
        {
            weeklyGoalMinutes = weeklyGoal;
        }

        return new WeeklyActivitySummaryResponseDto
        {
            TotalMinutes = activities.Sum(x => x.DurationMinutes),
            WeeklyGoalMinutes = weeklyGoalMinutes,
            Activities = activities
        };
    }
}