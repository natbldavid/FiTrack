using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.Meals.Requests;

public class UpdateMealRequestDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsFavorite { get; set; }

    [MinLength(1)]
    public List<MealItemRequestDto> Items { get; set; } = new();
}