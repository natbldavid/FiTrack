namespace FiTrack.Api.Dtos.Meals.Responses;

public class MealSummaryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsFavorite { get; set; }

    public int TotalCalories { get; set; }
    public decimal TotalProtein { get; set; }
    public decimal TotalCarbs { get; set; }
    public decimal TotalFat { get; set; }

    public int ItemCount { get; set; }
}