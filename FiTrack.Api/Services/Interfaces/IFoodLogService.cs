using FiTrack.Api.Dtos.FoodLogs.Requests;
using FiTrack.Api.Dtos.FoodLogs.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface IFoodLogService
{
    Task<FoodLogResponseDto> CreateFoodLogAsync(int userId, CreateFoodLogRequestDto request);
    Task<FoodLogResponseDto?> UpdateFoodLogAsync(int userId, int foodLogId, UpdateFoodLogRequestDto request);
    Task<DailyFoodSummaryResponseDto> GetDailySummaryAsync(int userId, DateOnly logDate);
}