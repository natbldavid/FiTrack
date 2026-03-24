namespace FiTrack.Api.Dtos.WeightLogs.Responses;

public class WeightTrendPointDto
{
    public DateOnly LogDate { get; set; }
    public decimal Weight { get; set; }
}