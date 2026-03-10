namespace FiTrack.Api.Dtos.FoodLogs.Responses;

public class FoodLogResponseDto
{
    public int Id { get; set; }
    public DateOnly LogDate { get; set; }
    public DateTime LoggedAt { get; set; }

    public string SourceType { get; set; } = null!;
    public int? FoodId { get; set; }
    public int? MealId { get; set; }

    public string NameSnapshot { get; set; } = null!;

    public int Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }

    public decimal Quantity { get; set; }
    public string? MealSlot { get; set; }
    public string? Note { get; set; }
}