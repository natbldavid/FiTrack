using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.WorkoutSessions.Requests;

public class CompleteWorkoutSessionRequestDto
{
    [StringLength(2000)]
    public string? Notes { get; set; }

    public List<UpdateWorkingWeightRequestDto> WorkingWeightUpdates { get; set; } = new();
}