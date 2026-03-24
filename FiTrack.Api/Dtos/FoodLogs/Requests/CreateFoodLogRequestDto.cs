using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.FoodLogs.Requests;

public class CreateFoodLogRequestDto
{
    [Required]
    public DateOnly LogDate { get; set; }

    [Required]
    public DateTime LoggedAt { get; set; }

    [Required]
    [StringLength(20)]
    public string SourceType { get; set; } = null!; // food | meal | custom

    public int? FoodId { get; set; }
    public int? MealId { get; set; }

    [StringLength(100)]
    public string? CustomName { get; set; }

    [Range(0, 10000)]
    public int? Calories { get; set; }

    [Range(typeof(decimal), "0", "10000")]
    public decimal? Protein { get; set; }

    [Range(typeof(decimal), "0", "10000")]
    public decimal? Carbs { get; set; }

    [Range(typeof(decimal), "0", "10000")]
    public decimal? Fat { get; set; }

    [Range(typeof(decimal), "0.01", "10000")]
    public decimal Quantity { get; set; } = 1;

    [StringLength(20)]
    public string? MealSlot { get; set; } // breakfast/lunch/dinner/snack

    [StringLength(500)]
    public string? Note { get; set; }
}