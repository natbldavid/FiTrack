using FiTrack.Api.Data;
using FiTrack.Api.Dtos.Food.Requests;
using FiTrack.Api.Dtos.Food.Responses;
using FiTrack.Api.Models.Food;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class FoodService : IFoodService
{
    private readonly FiTrackDbContext _context;

    public FoodService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<FoodResponseDto> CreateFoodAsync(int userId, CreateFoodRequestDto request)
    {
        var userExists = await _context.Users
            .AnyAsync(u => u.Id == userId);

        if (!userExists)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var now = DateTime.UtcNow;

        var food = new Food
        {
            UserId = userId,
            Name = request.Name,
            ServingDescription = request.ServingDescription,
            Calories = request.Calories,
            Protein = request.Protein,
            Carbs = request.Carbs,
            Fat = request.Fat,
            IsFavorite = request.IsFavorite,
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.Foods.Add(food);
        await _context.SaveChangesAsync();

        return new FoodResponseDto
        {
            Id = food.Id,
            UserId = food.UserId,
            Name = food.Name,
            ServingDescription = food.ServingDescription,
            Calories = food.Calories,
            Protein = food.Protein,
            Carbs = food.Carbs,
            Fat = food.Fat,
            IsFavorite = food.IsFavorite,
            CreatedAt = food.CreatedAt,
            UpdatedAt = food.UpdatedAt
        };
    }
    public async Task<FoodResponseDto?> UpdateFoodAsync(int userId, int foodId, UpdateFoodRequestDto request)
    {
        var food = await _context.Foods
            .FirstOrDefaultAsync(f => f.Id == foodId && f.UserId == userId);

        if (food is null)
        {
            return null;
        }

        food.Name = request.Name;
        food.ServingDescription = request.ServingDescription;
        food.Calories = request.Calories;
        food.Protein = request.Protein;
        food.Carbs = request.Carbs;
        food.Fat = request.Fat;
        food.IsFavorite = request.IsFavorite;

        food.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new FoodResponseDto
        {
            Id = food.Id,
            UserId = food.UserId,
            Name = food.Name,
            ServingDescription = food.ServingDescription,
            Calories = food.Calories,
            Protein = food.Protein,
            Carbs = food.Carbs,
            Fat = food.Fat,
            IsFavorite = food.IsFavorite,
            CreatedAt = food.CreatedAt,
            UpdatedAt = food.UpdatedAt
        };
    }
    public async Task<List<FoodSummaryResponseDto>> GetFoodsAsync(int userId)
    {
        return await _context.Foods
            .Where(f => f.UserId == userId)
            .OrderBy(f => f.Name)
            .Select(f => new FoodSummaryResponseDto
            {
                Id = f.Id,
                Name = f.Name,
                ServingDescription = f.ServingDescription,
                Calories = f.Calories,
                Protein = f.Protein,
                Carbs = f.Carbs,
                Fat = f.Fat,
                IsFavorite = f.IsFavorite
            })
            .ToListAsync();
    }
}