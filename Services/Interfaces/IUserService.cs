using FiTrack.Api.Dtos.Users.Requests;
using FiTrack.Api.Dtos.Users.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> RegisterUserAsync(RegisterUserRequestDto request);
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

    Task<UserProfileResponseDto?> GetUserProfileAsync(int userId);

    Task<UserProfileResponseDto?> UpdateUserProfileAsync(int userId, UpdateUserProfileRequestDto request);

    Task<bool> ChangePasscodeAsync(int userId, ChangePasscodeRequestDto request);
}