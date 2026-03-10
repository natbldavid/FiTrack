using System.ComponentModel.DataAnnotations;

namespace FiTrack.Api.Dtos.Meals.Requests;

public class MealItemRequestDto
{
    [Range(1, int.MaxValue)]
    public int FoodId { get; set; }

    [Range(typeof(decimal), "0.01", "10000")]
    public decimal Quantity { get; set; }
}