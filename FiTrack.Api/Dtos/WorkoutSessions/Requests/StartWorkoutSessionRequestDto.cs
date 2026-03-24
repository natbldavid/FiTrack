using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.WorkoutSessions.Requests;

public class StartWorkoutSessionRequestDto
{
    [Range(1, int.MaxValue)]
    public int WorkoutDayId { get; set; }
}