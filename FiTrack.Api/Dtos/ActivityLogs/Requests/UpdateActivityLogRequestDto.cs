using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.ActivityLogs.Requests;

public class UpdateActivityLogRequestDto
{
    [Range(1, int.MaxValue)]
    public int ActivityTypeId { get; set; }

    [Required]
    public DateOnly LogDate { get; set; }

    [Range(1, 1440)]
    public int DurationMinutes { get; set; }

    [Range(typeof(decimal), "0", "10000")]
    public decimal? Distance { get; set; }

    [Range(0, 100000)]
    public int? CaloriesBurned { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }
}