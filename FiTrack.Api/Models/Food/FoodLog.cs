namespace FiTrack.Api.Models.Food;

public class FoodLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateOnly LogDate { get; set; }

    public DateTime LoggedAt { get; set; }

    public string SourceType { get; set; } = null!;

    public int? FoodId { get; set; }

    public int? MealId { get; set; }

    public string NameSnapshot { get; set; } = null!;
    public string? ServingDescriptionSnapshot { get; set; }

    public int Calories { get; set; }

    public decimal Protein { get; set; }

    public decimal Carbs { get; set; }

    public decimal Fat { get; set; }

    public decimal Quantity { get; set; }

    public string? MealSlot { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Users.User User { get; set; } = null!;
    public Food? Food { get; set; }
    public Meal? Meal { get; set; }
}