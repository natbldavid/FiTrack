using FiTrack.Api.Dtos.WorkoutSessions.Requests;
using FiTrack.Api.Dtos.WorkoutSessions.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface IWorkoutSessionService
{
    Task<WorkoutSessionResponseDto> StartWorkoutSessionAsync(int userId, StartWorkoutSessionRequestDto request);
    Task<WorkoutSessionResponseDto?> GetWorkoutSessionByIdAsync(int userId, int workoutSessionId);
    Task<List<WorkoutSessionSummaryResponseDto>> GetWorkoutSessionsAsync(int userId);
    Task<WorkoutSessionResponseDto?> UpsertWorkoutSetLogAsync(int userId, int workoutSessionId, UpdateWorkoutSetLogRequestDto request);
    Task<WorkoutSessionResponseDto?> CompleteWorkoutSessionAsync(int userId, int workoutSessionId, CompleteWorkoutSessionRequestDto request);
}