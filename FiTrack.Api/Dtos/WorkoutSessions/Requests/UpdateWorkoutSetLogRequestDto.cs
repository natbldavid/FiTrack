using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.WorkoutSessions.Requests;

public class UpdateWorkoutSetLogRequestDto
{
    [Range(1, int.MaxValue)]
    public int WorkoutSessionExerciseId { get; set; }

    [Range(1, 100)]
    public int SetNumber { get; set; }

    [Range(0, 1000)]
    public int Reps { get; set; }

    [Range(typeof(decimal), "0", "10000")]
    public decimal? Weight { get; set; }

    public bool Completed { get; set; }
}