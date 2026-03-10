namespace FiTrack.Api.Models.Activities;

public class ActivityType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
}