using FiTrack.Api.Dtos.WorkoutDays.Requests;
using FiTrack.Api.Dtos.WorkoutDays.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface IWorkoutDayService
{
    Task<WorkoutDayDetailResponseDto> CreateWorkoutDayAsync(int userId, CreateWorkoutDayRequestDto request);
    Task<WorkoutDayDetailResponseDto?> UpdateWorkoutDayAsync(int userId, int workoutDayId, UpdateWorkoutDayRequestDto request);
    Task<List<WorkoutDaySummaryResponseDto>> GetWorkoutDaysAsync(int userId);
    Task<WorkoutDayDetailResponseDto?> GetWorkoutDayByIdAsync(int userId, int workoutDayId);
}