namespace FiTrack.Api.Models.Users;

public class WeightLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateOnly LogDate { get; set; }

    public DateTime LoggedAt { get; set; }

    public decimal Weight { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
}