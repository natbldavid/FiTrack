using FiTrack.Api.Dtos.WeightLogs.Requests;
using FiTrack.Api.Dtos.WeightLogs.Responses;

namespace FiTrack.Api.Services.Interfaces;

public interface IWeightLogService
{
    Task<WeightLogResponseDto> CreateWeightLogAsync(int userId, CreateWeightLogRequestDto request);

    Task<WeightLogResponseDto?> UpdateWeightLogAsync(int userId, int weightLogId, UpdateWeightLogRequestDto request);

    Task<List<WeightTrendPointDto>> GetWeightTrendAsync(int userId);
}