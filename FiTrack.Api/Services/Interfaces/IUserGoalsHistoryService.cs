using FiTrack.Api.Dtos.Users.Requests;
using FiTrack.Api.Dtos.Users.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface IUserGoalsHistoryService
{
    Task<IEnumerable<UserGoalsHistoryResponseDto>> GetGoalsHistoryAsync(int userId);

    Task<UserGoalsHistoryResponseDto> CreateGoalsHistoryAsync(
        int userId,
        CreateUserGoalsHistoryRequestDto request);

    Task<UserGoalsHistoryResponseDto?> UpdateGoalsHistoryAsync(
        int userId,
        int id,
        UpdateUserGoalsHistoryRequestDto request);
}