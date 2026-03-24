using FiTrack.Api.Data;
using FiTrack.Api.Dtos.FoodLogs.Requests;
using FiTrack.Api.Dtos.FoodLogs.Responses;
using FiTrack.Api.Models.Food;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class FoodLogService : IFoodLogService
{
    private readonly FiTrackDbContext _context;

    public FoodLogService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<FoodLogResponseDto> CreateFoodLogAsync(int userId, CreateFoodLogRequestDto request)
    {
        var now = DateTime.UtcNow;

        var sourceType = request.SourceType.Trim().ToLowerInvariant();

        string nameSnapshot;
        string? servingDescriptionSnapshot = null;
        int calories;
        decimal protein;
        decimal carbs;
        decimal fat;

        int? foodId = null;
        int? mealId = null;

        switch (sourceType)
        {
            case "food":
                {
                    if (!request.FoodId.HasValue)
                    {
                        throw new InvalidOperationException("FoodId is required when SourceType is 'food'.");
                    }

                    var food = await _context.Foods
                        .FirstOrDefaultAsync(f => f.Id == request.FoodId.Value && f.UserId == userId);

                    if (food is null)
                    {
                        throw new KeyNotFoundException("Food not found.");
                    }

                    nameSnapshot = food.Name;
                    servingDescriptionSnapshot = food.ServingDescription;
                    calories = (int)Math.Round(food.Calories * request.Quantity, MidpointRounding.AwayFromZero);
                    protein = food.Protein * request.Quantity;
                    carbs = food.Carbs * request.Quantity;
                    fat = food.Fat * request.Quantity;
                    foodId = food.Id;

                    break;
                }

            case "meal":
                {
                    if (!request.MealId.HasValue)
                    {
                        throw new InvalidOperationException("MealId is required when SourceType is 'meal'.");
                    }

                    var meal = await _context.Meals
                        .Include(m => m.MealItems)
                            .ThenInclude(mi => mi.Food)
                        .FirstOrDefaultAsync(m => m.Id == request.MealId.Value && m.UserId == userId);

                    if (meal is null)
                    {
                        throw new KeyNotFoundException("Meal not found.");
                    }

                    nameSnapshot = meal.Name;
                    calories = (int)Math.Round(
                        meal.MealItems.Sum(i => i.Food.Calories * i.Quantity) * request.Quantity,
                        MidpointRounding.AwayFromZero);
                    protein = meal.MealItems.Sum(i => i.Food.Protein * i.Quantity) * request.Quantity;
                    carbs = meal.MealItems.Sum(i => i.Food.Carbs * i.Quantity) * request.Quantity;
                    fat = meal.MealItems.Sum(i => i.Food.Fat * i.Quantity) * request.Quantity;
                    mealId = meal.Id;

                    break;
                }

            case "custom":
                {
                    if (string.IsNullOrWhiteSpace(request.CustomName))
                    {
                        throw new InvalidOperationException("CustomName is required when SourceType is 'custom'.");
                    }

                    if (!request.Calories.HasValue ||
                        !request.Protein.HasValue ||
                        !request.Carbs.HasValue ||
                        !request.Fat.HasValue)
                    {
                        throw new InvalidOperationException("Calories, Protein, Carbs and Fat are required for custom entries.");
                    }

                    nameSnapshot = request.CustomName.Trim();
                    calories = (int)Math.Round(request.Calories.Value * request.Quantity, MidpointRounding.AwayFromZero);
                    protein = request.Protein.Value * request.Quantity;
                    carbs = request.Carbs.Value * request.Quantity;
                    fat = request.Fat.Value * request.Quantity;

                    break;
                }

            default:
                throw new InvalidOperationException("SourceType must be 'food', 'meal', or 'custom'.");
        }

        var foodLog = new FoodLog
        {
            UserId = userId,
            LogDate = request.LogDate,
            LoggedAt = request.LoggedAt,
            SourceType = sourceType,
            FoodId = foodId,
            MealId = mealId,
            NameSnapshot = nameSnapshot,
            ServingDescriptionSnapshot = servingDescriptionSnapshot,
            Calories = calories,
            Protein = protein,
            Carbs = carbs,
            Fat = fat,
            Quantity = request.Quantity,
            MealSlot = request.MealSlot,
            Note = request.Note,
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.FoodLogs.Add(foodLog);
        await _context.SaveChangesAsync();

        return MapToFoodLogResponseDto(foodLog);
    }

    public async Task<FoodLogResponseDto?> UpdateFoodLogAsync(int userId, int foodLogId, UpdateFoodLogRequestDto request)
    {
        var foodLog = await _context.FoodLogs
            .FirstOrDefaultAsync(fl => fl.Id == foodLogId && fl.UserId == userId);

        if (foodLog is null)
        {
            return null;
        }

        foodLog.Quantity = request.Quantity;
        foodLog.MealSlot = request.MealSlot;
        foodLog.Note = request.Note;

        if (foodLog.SourceType == "custom")
        {
            foodLog.Calories = request.Calories ?? foodLog.Calories;
            foodLog.Protein = request.Protein ?? foodLog.Protein;
            foodLog.Carbs = request.Carbs ?? foodLog.Carbs;
            foodLog.Fat = request.Fat ?? foodLog.Fat;
        }

        foodLog.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToFoodLogResponseDto(foodLog);
    }

    public async Task<DailyFoodSummaryResponseDto> GetDailySummaryAsync(int userId, DateOnly logDate)
    {
        var items = await _context.FoodLogs
            .Where(fl => fl.UserId == userId && fl.LogDate == logDate)
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
                ServingDescriptionSnapshot = fl.ServingDescriptionSnapshot,
                Calories = fl.Calories,
                Protein = fl.Protein,
                Carbs = fl.Carbs,
                Fat = fl.Fat,
                Quantity = fl.Quantity,
                MealSlot = fl.MealSlot,
                Note = fl.Note
            })
            .ToListAsync();

        return new DailyFoodSummaryResponseDto
        {
            LogDate = logDate,
            TotalCalories = items.Sum(i => i.Calories),
            TotalProtein = items.Sum(i => i.Protein),
            TotalCarbs = items.Sum(i => i.Carbs),
            TotalFat = items.Sum(i => i.Fat),
            Items = items
        };
    }

    private static FoodLogResponseDto MapToFoodLogResponseDto(FoodLog foodLog)
    {
        return new FoodLogResponseDto
        {
            Id = foodLog.Id,
            LogDate = foodLog.LogDate,
            LoggedAt = foodLog.LoggedAt,
            SourceType = foodLog.SourceType,
            FoodId = foodLog.FoodId,
            MealId = foodLog.MealId,
            NameSnapshot = foodLog.NameSnapshot,
            ServingDescriptionSnapshot = foodLog.ServingDescriptionSnapshot,
            Calories = foodLog.Calories,
            Protein = foodLog.Protein,
            Carbs = foodLog.Carbs,
            Fat = foodLog.Fat,
            Quantity = foodLog.Quantity,
            MealSlot = foodLog.MealSlot,
            Note = foodLog.Note
        };
    }
}