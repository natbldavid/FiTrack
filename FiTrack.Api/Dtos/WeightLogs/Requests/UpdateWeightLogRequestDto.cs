using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.WeightLogs.Requests;

public class UpdateWeightLogRequestDto
{
    [Range(1, 1000)]
    public decimal Weight { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }
}