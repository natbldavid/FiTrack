namespace FiTrack.Api.Dtos.WorkoutSessions.Responses;

public class WorkoutSetLogResponseDto
{
    public int Id { get; set; }
    public int SetNumber { get; set; }
    public int Reps { get; set; }
    public decimal? Weight { get; set; }
    public bool Completed { get; set; }
}