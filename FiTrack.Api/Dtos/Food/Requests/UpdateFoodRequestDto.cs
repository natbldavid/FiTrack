using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.Food.Requests;

public class UpdateFoodRequestDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string? ServingDescription { get; set; }

    [Range(0, 10000)]
    public int Calories { get; set; }

    [Range(typeof(decimal), "0", "1000")]
    public decimal Protein { get; set; }

    [Range(typeof(decimal), "0", "1000")]
    public decimal Carbs { get; set; }

    [Range(typeof(decimal), "0", "1000")]
    public decimal Fat { get; set; }

    public bool IsFavorite { get; set; }
}