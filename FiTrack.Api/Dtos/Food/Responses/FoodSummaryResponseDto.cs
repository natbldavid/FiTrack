namespace FiTrack.Api.Dtos.Food.Responses;

public class FoodSummaryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ServingDescription { get; set; }

    public int Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }

    public bool IsFavorite { get; set; }
}