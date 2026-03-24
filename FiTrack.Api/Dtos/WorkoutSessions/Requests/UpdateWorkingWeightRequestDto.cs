using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.WorkoutSessions.Requests;

public class UpdateWorkingWeightRequestDto
{
    [Range(1, int.MaxValue)]
    public int WorkoutDayExerciseId { get; set; }

    [Range(typeof(decimal), "0", "10000")]
    public decimal CurrentWorkingWeight { get; set; }
}