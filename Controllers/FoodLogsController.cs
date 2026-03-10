using System.Security.Claims;
using FiTrack.Api.Dtos.FoodLogs.Requests;
using FiTrack.Api.Dtos.FoodLogs.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FoodLogsController : ControllerBase
{
    private readonly IFoodLogService _foodLogService;

    public FoodLogsController(IFoodLogService foodLogService)
    {
        _foodLogService = foodLogService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(FoodLogResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FoodLogResponseDto>> Create([FromBody] CreateFoodLogRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var created = await _foodLogService.CreateFoodLogAsync(userId, request);
            return CreatedAtAction(nameof(GetDailySummary), new { logDate = created.LogDate }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(FoodLogResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FoodLogResponseDto>> Update(int id, [FromBody] UpdateFoodLogRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var updated = await _foodLogService.UpdateFoodLogAsync(userId, id, request);

        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpGet("daily/{logDate}")]
    [ProducesResponseType(typeof(DailyFoodSummaryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<DailyFoodSummaryResponseDto>> GetDailySummary(DateOnly logDate)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var summary = await _foodLogService.GetDailySummaryAsync(userId, logDate);
        return Ok(summary);
    }

    private bool TryGetUserId(out int userId)
    {
        userId = 0;

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return !string.IsNullOrWhiteSpace(userIdClaim)
            && int.TryParse(userIdClaim, out userId);
    }
}