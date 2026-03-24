using FiTrack.Api.Data;
using FiTrack.Api.Dtos.Users.Requests;
using FiTrack.Api.Dtos.Users.Responses;
using FiTrack.Api.Models.Users;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class UserGoalsHistoryService : IUserGoalsHistoryService
{
    private readonly FiTrackDbContext _context;

    public UserGoalsHistoryService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserGoalsHistoryResponseDto>> GetGoalsHistoryAsync(int userId)
    {
        var rows = await _context.UserGoalsHistory
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.EffectiveFrom)
            .ThenByDescending(x => x.Id)
            .ToListAsync();

        return rows.Select(MapToDto);
    }

    public async Task<UserGoalsHistoryResponseDto> CreateGoalsHistoryAsync(
    int userId,
    CreateUserGoalsHistoryRequestDto request)
    {
        var now = DateTime.UtcNow;
        var todayStart = now.Date;

        var userExists = await _context.Users.AnyAsync(x => x.Id == userId);
        if (!userExists)
        {
            throw new KeyNotFoundException("User not found.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        var activeRow = await _context.UserGoalsHistory
            .Where(x => x.UserId == userId && x.EffectiveTo == null)
            .OrderByDescending(x => x.EffectiveFrom)
            .FirstOrDefaultAsync();

        if (activeRow is not null)
        {
            activeRow.EffectiveTo = todayStart.AddTicks(-1);
        }

        var newRow = new UserGoalsHistory
        {
            UserId = userId,
            EffectiveFrom = todayStart,
            EffectiveTo = null,
            DailyCalorieGoal = request.DailyCalorieGoal,
            DailyProteinGoal = request.DailyProteinGoal,
            DailyCarbGoal = request.DailyCarbGoal,
            DailyFatGoal = request.DailyFatGoal,
            WeightGoal = request.WeightGoal,
            WeeklyExerciseGoal = request.WeeklyExerciseGoal,
            CreatedAt = now
        };

        _context.UserGoalsHistory.Add(newRow);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return MapToDto(newRow);
    }

    public async Task<UserGoalsHistoryResponseDto?> UpdateGoalsHistoryAsync(
        int userId,
        int id,
        UpdateUserGoalsHistoryRequestDto request)
    {
        var row = await _context.UserGoalsHistory
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (row is null)
        {
            return null;
        }

        row.EffectiveFrom = request.EffectiveFrom;
        row.EffectiveTo = request.EffectiveTo;
        row.DailyCalorieGoal = request.DailyCalorieGoal;
        row.DailyProteinGoal = request.DailyProteinGoal;
        row.DailyCarbGoal = request.DailyCarbGoal;
        row.DailyFatGoal = request.DailyFatGoal;
        row.WeightGoal = request.WeightGoal;
        row.WeeklyExerciseGoal = request.WeeklyExerciseGoal;

        await _context.SaveChangesAsync();

        return MapToDto(row);
    }

    private static UserGoalsHistoryResponseDto MapToDto(UserGoalsHistory entity)
    {
        return new UserGoalsHistoryResponseDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            EffectiveFrom = entity.EffectiveFrom,
            EffectiveTo = entity.EffectiveTo,
            DailyCalorieGoal = entity.DailyCalorieGoal,
            DailyProteinGoal = entity.DailyProteinGoal,
            DailyCarbGoal = entity.DailyCarbGoal,
            DailyFatGoal = entity.DailyFatGoal,
            WeightGoal = entity.WeightGoal,
            WeeklyExerciseGoal = entity.WeeklyExerciseGoal,
            CreatedAt = entity.CreatedAt
        };
    }
}