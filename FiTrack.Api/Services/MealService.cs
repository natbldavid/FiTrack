using FiTrack.Api.Data;
using FiTrack.Api.Dtos.Meals.Requests;
using FiTrack.Api.Dtos.Meals.Responses;
using FiTrack.Api.Models.Food;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class MealService : IMealService
{
    private readonly FiTrackDbContext _context;

    public MealService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<MealDetailResponseDto> CreateMealAsync(int userId, CreateMealRequestDto request)
    {
        var foodIds = request.Items.Select(i => i.FoodId).Distinct().ToList();

        var foods = await _context.Foods
            .Where(f => f.UserId == userId && foodIds.Contains(f.Id))
            .ToListAsync();

        if (foods.Count != foodIds.Count)
        {
            throw new KeyNotFoundException("One or more foods were not found.");
        }

        var now = DateTime.UtcNow;

        var meal = new Meal
        {
            UserId = userId,
            Name = request.Name,
            Description = request.Description,
            IsFavorite = request.IsFavorite,
            CreatedAt = now,
            UpdatedAt = now,
            MealItems = request.Items.Select(i => new MealItem
            {
                FoodId = i.FoodId,
                Quantity = i.Quantity,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList()
        };

        _context.Meals.Add(meal);
        await _context.SaveChangesAsync();

        return await GetMealByIdAsync(userId, meal.Id)
            ?? throw new InvalidOperationException("Meal was created but could not be loaded.");
    }

    public async Task<MealDetailResponseDto?> UpdateMealAsync(int userId, int mealId, UpdateMealRequestDto request)
    {
        var meal = await _context.Meals
            .Include(m => m.MealItems)
            .FirstOrDefaultAsync(m => m.Id == mealId && m.UserId == userId);

        if (meal is null)
        {
            return null;
        }

        var foodIds = request.Items.Select(i => i.FoodId).Distinct().ToList();

        var foods = await _context.Foods
            .Where(f => f.UserId == userId && foodIds.Contains(f.Id))
            .ToListAsync();

        if (foods.Count != foodIds.Count)
        {
            throw new KeyNotFoundException("One or more foods were not found.");
        }

        var now = DateTime.UtcNow;

        meal.Name = request.Name;
        meal.Description = request.Description;
        meal.IsFavorite = request.IsFavorite;
        meal.UpdatedAt = now;

        _context.MealItems.RemoveRange(meal.MealItems);

        meal.MealItems = request.Items.Select(i => new MealItem
        {
            MealId = meal.Id,
            FoodId = i.FoodId,
            Quantity = i.Quantity,
            CreatedAt = now,
            UpdatedAt = now
        }).ToList();

        await _context.SaveChangesAsync();

        return await GetMealByIdAsync(userId, meal.Id);
    }

    public async Task<List<MealSummaryResponseDto>> GetMealsAsync(int userId)
    {
        var meals = await _context.Meals
            .Where(m => m.UserId == userId)
            .Include(m => m.MealItems)
                .ThenInclude(mi => mi.Food)
            .OrderBy(m => m.Name)
            .ToListAsync();

        return meals.Select(MapToMealSummaryResponseDto).ToList();
    }

    public async Task<MealDetailResponseDto?> GetMealByIdAsync(int userId, int mealId)
    {
        var meal = await _context.Meals
            .Include(m => m.MealItems)
                .ThenInclude(mi => mi.Food)
            .FirstOrDefaultAsync(m => m.Id == mealId && m.UserId == userId);

        if (meal is null)
        {
            return null;
        }

        return MapToMealDetailResponseDto(meal);
    }

    private static MealSummaryResponseDto MapToMealSummaryResponseDto(Meal meal)
    {
        var totalCalories = meal.MealItems.Sum(i => (int)Math.Round(i.Food.Calories * i.Quantity, MidpointRounding.AwayFromZero));
        var totalProtein = meal.MealItems.Sum(i => i.Food.Protein * i.Quantity);
        var totalCarbs = meal.MealItems.Sum(i => i.Food.Carbs * i.Quantity);
        var totalFat = meal.MealItems.Sum(i => i.Food.Fat * i.Quantity);

        return new MealSummaryResponseDto
        {
            Id = meal.Id,
            Name = meal.Name,
            Description = meal.Description,
            IsFavorite = meal.IsFavorite,
            TotalCalories = totalCalories,
            TotalProtein = totalProtein,
            TotalCarbs = totalCarbs,
            TotalFat = totalFat,
            ItemCount = meal.MealItems.Count
        };
    }

    private static MealDetailResponseDto MapToMealDetailResponseDto(Meal meal)
    {
        var items = meal.MealItems.Select(i => new MealItemResponseDto
        {
            Id = i.Id,
            FoodId = i.FoodId,
            FoodName = i.Food.Name,
            Quantity = i.Quantity,
            Calories = (int)Math.Round(i.Food.Calories * i.Quantity, MidpointRounding.AwayFromZero),
            Protein = i.Food.Protein * i.Quantity,
            Carbs = i.Food.Carbs * i.Quantity,
            Fat = i.Food.Fat * i.Quantity
        }).ToList();

        return new MealDetailResponseDto
        {
            Id = meal.Id,
            Name = meal.Name,
            Description = meal.Description,
            IsFavorite = meal.IsFavorite,
            TotalCalories = items.Sum(i => i.Calories),
            TotalProtein = items.Sum(i => i.Protein),
            TotalCarbs = items.Sum(i => i.Carbs),
            TotalFat = items.Sum(i => i.Fat),
            Items = items
        };
    }
}