using FiTrack.Api.Data;
using FiTrack.Api.Dtos.ExerciseCatalog.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Api.Services;

public class ExerciseCatalogService : IExerciseCatalogService
{
    private readonly FiTrackDbContext _context;

    public ExerciseCatalogService(FiTrackDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExerciseCatalogResponseDto>> GetExercisesAsync()
    {
        return await _context.ExerciseCatalog
            .Where(e => e.IsActive)
            .OrderBy(e => e.BodyPart)
            .ThenBy(e => e.Name)
            .Select(e => new ExerciseCatalogResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                BodyPart = e.BodyPart,
                ExerciseType = e.ExerciseType,
                ExerciseDemoGif = e.ExerciseDemoGif,
                IsActive = e.IsActive
            })
            .ToListAsync();
    }
}