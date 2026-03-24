namespace FiTrack.Api.Dtos.FoodLogs.Responses;

public class DailyFoodSummaryResponseDto
{
    public DateOnly LogDate { get; set; }

    public int TotalCalories { get; set; }
    public decimal TotalProtein { get; set; }
    public decimal TotalCarbs { get; set; }
    public decimal TotalFat { get; set; }

    public List<FoodLogResponseDto> Items { get; set; } = new();
}