namespace FiTrack.Api.Dtos.Meals.Responses;

public class MealItemResponseDto
{
    public int Id { get; set; }
    public int FoodId { get; set; }
    public string FoodName { get; set; } = null!;
    public decimal Quantity { get; set; }

    public int Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }
}