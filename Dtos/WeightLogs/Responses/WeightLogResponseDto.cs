namespace FiTrack.Api.Dtos.WeightLogs.Responses;

public class WeightLogResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public DateOnly LogDate { get; set; }
    public DateTime LoggedAt { get; set; }
    public decimal Weight { get; set; }
    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}