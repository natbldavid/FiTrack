using FiTrack.Api.Dtos.ActivityTypes.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface IActivityTypeService
{
    Task<List<ActivityTypeResponseDto>> GetActivityTypesAsync();
}