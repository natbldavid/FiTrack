namespace FiTrack.Api.Models.Food;

public class MealItem
{
    public int Id { get; set; }

    public int MealId { get; set; }

    public int FoodId { get; set; }

    public decimal Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Meal Meal { get; set; } = null!;

    public Food Food { get; set; } = null!;
}