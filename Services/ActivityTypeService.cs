using FiTrack.Api.Data;
using FiTrack.Api.Dtos.ActivityTypes.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class ActivityTypeService : IActivityTypeService
{
    private readonly FiTrackDbContext _context;

    public ActivityTypeService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<List<ActivityTypeResponseDto>> GetActivityTypesAsync()
    {
        return await _context.Set<FiTrack.Api.Models.Activities.ActivityType>()
            .OrderBy(x => x.Name)
            .Select(x => new ActivityTypeResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon
            })
            .ToListAsync();
    }
}