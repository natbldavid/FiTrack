using FiTrack.Api.Dtos.Meals.Requests;
using FiTrack.Api.Dtos.Meals.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface IMealService
{
    Task<MealDetailResponseDto> CreateMealAsync(int userId, CreateMealRequestDto request);
    Task<MealDetailResponseDto?> UpdateMealAsync(int userId, int mealId, UpdateMealRequestDto request);
    Task<List<MealSummaryResponseDto>> GetMealsAsync(int userId);
    Task<MealDetailResponseDto?> GetMealByIdAsync(int userId, int mealId);
}