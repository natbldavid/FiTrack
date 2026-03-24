using FiTrack.Api.Dtos.Today.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface ITodayService
{
    Task<TodayDashboardResponseDto> GetTodayDashboardAsync(int userId, DateOnly date);
}