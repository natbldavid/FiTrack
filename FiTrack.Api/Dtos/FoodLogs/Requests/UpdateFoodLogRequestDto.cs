using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.FoodLogs.Requests;

public class UpdateFoodLogRequestDto
{
    [Range(typeof(decimal), "0.01", "10000")]
    public decimal Quantity { get; set; }

    [StringLength(20)]
    public string? MealSlot { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    [Range(0, 10000)]
    public int? Calories { get; set; }

    [Range(typeof(decimal), "0", "10000")]
    public decimal? Protein { get; set; }

    [Range(typeof(decimal), "0", "10000")]
    public decimal? Carbs { get; set; }

    [Range(typeof(decimal), "0", "10000")]
    public decimal? Fat { get; set; }
}