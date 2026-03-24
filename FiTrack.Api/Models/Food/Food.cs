namespace FiTrack.Api.Models.Food;

public class Food
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? ServingDescription { get; set; }

    public int Calories { get; set; }

    public decimal Protein { get; set; }

    public decimal Carbs { get; set; }

    public decimal Fat { get; set; }

    public bool IsFavorite { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Users.User User { get; set; } = null!;

    public ICollection<MealItem> MealItems { get; set; } = new List<MealItem>();

    public ICollection<FoodLog> FoodLogs { get; set; } = new List<FoodLog>();
}