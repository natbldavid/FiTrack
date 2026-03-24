namespace FiTrack.Api.Dtos.ExerciseCatalog.Responses;

public class ExerciseCatalogResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string BodyPart { get; set; } = null!;
    public string ExerciseType { get; set; } = null!;
    public string? ExerciseDemoGif { get; set; }
    public bool IsActive { get; set; }
}