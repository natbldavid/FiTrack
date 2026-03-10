using FiTrack.Api.Dtos.ActivityLogs.Requests;
using FiTrack.Api.Dtos.ActivityLogs.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface IActivityLogService
{
    Task<ActivityLogResponseDto> CreateActivityLogAsync(int userId, CreateActivityLogRequestDto request);
    Task<ActivityLogResponseDto?> UpdateActivityLogAsync(int userId, int activityLogId, UpdateActivityLogRequestDto request);
    Task<List<ActivityLogResponseDto>> GetActivityLogsAsync(int userId);
    Task<WeeklyActivitySummaryResponseDto> GetWeeklySummaryAsync(int userId, DateOnly weekStartDate);
}