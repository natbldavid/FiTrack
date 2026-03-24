using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.WorkoutDays.Requests;

public class WorkoutDayExerciseRequestDto
{
    [Range(1, int.MaxValue)]
    public int ExerciseId { get; set; }

    [Range(1, int.MaxValue)]
    public int ExerciseOrder { get; set; }

    [Range(1, 100)]
    public int TargetSets { get; set; }

    [Range(1, 100)]
    public int TargetRepsMin { get; set; }

    [Range(1, 100)]
    public int TargetRepsMax { get; set; }

    [Range(typeof(decimal), "0", "10000")]
    public decimal? InitialWeight { get; set; }

    [Range(typeof(decimal), "0", "10000")]
    public decimal? CurrentWorkingWeight { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }
}