using FiTrack.Api.Dtos.ExerciseCatalog.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface IExerciseCatalogService
{
    Task<List<ExerciseCatalogResponseDto>> GetExercisesAsync();
}