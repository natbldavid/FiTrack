using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.WorkoutDays.Requests;

public class CreateWorkoutDayRequestDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string? MuscleFocus { get; set; }

    public int? SortOrder { get; set; }

    [MinLength(1)]
    public List<WorkoutDayExerciseRequestDto> Exercises { get; set; } = new();
}