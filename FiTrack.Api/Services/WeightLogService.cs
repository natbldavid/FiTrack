using FiTrack.Api.Data;
using FiTrack.Api.Dtos.WeightLogs.Requests;
using FiTrack.Api.Dtos.WeightLogs.Responses;
using FiTrack.Api.Models.Users;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class WeightLogService : IWeightLogService
{
    private readonly FiTrackDbContext _context;

    public WeightLogService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<WeightLogResponseDto> CreateWeightLogAsync(int userId, CreateWeightLogRequestDto request)
    {
        var userExists = await _context.Users
            .AnyAsync(u => u.Id == userId);

        if (!userExists)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var now = DateTime.UtcNow;

        var weightLog = new WeightLog
        {
            UserId = userId,
            LogDate = request.LogDate,
            LoggedAt = request.LoggedAt,
            Weight = request.Weight,
            Note = request.Note,
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.WeightLogs.Add(weightLog);
        await _context.SaveChangesAsync();

        return new WeightLogResponseDto
        {
            Id = weightLog.Id,
            UserId = weightLog.UserId,
            LogDate = weightLog.LogDate,
            LoggedAt = weightLog.LoggedAt,
            Weight = weightLog.Weight,
            Note = weightLog.Note,
            CreatedAt = weightLog.CreatedAt,
            UpdatedAt = weightLog.UpdatedAt
        };
    }
    public async Task<WeightLogResponseDto?> UpdateWeightLogAsync(int userId, int weightLogId, UpdateWeightLogRequestDto request)
    {
        var weightLog = await _context.WeightLogs
            .FirstOrDefaultAsync(w => w.Id == weightLogId && w.UserId == userId);

        if (weightLog is null)
        {
            return null;
        }

        weightLog.Weight = request.Weight;
        weightLog.Note = request.Note;
        weightLog.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new WeightLogResponseDto
        {
            Id = weightLog.Id,
            UserId = weightLog.UserId,
            LogDate = weightLog.LogDate,
            LoggedAt = weightLog.LoggedAt,
            Weight = weightLog.Weight,
            Note = weightLog.Note,
            CreatedAt = weightLog.CreatedAt,
            UpdatedAt = weightLog.UpdatedAt
        };
    }
    public async Task<List<WeightTrendPointDto>> GetWeightTrendAsync(int userId)
    {
        return await _context.WeightLogs
            .Where(w => w.UserId == userId)
            .OrderBy(w => w.LogDate)
            .Select(w => new WeightTrendPointDto
            {
                LogDate = w.LogDate,
                Weight = w.Weight
            })
            .ToListAsync();
    }
}