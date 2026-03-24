using System.Security.Claims;
using FiTrack.Api.Dtos.ActivityLogs.Requests;
using FiTrack.Api.Dtos.ActivityLogs.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivityLogsController : ControllerBase
{
    private readonly IActivityLogService _activityLogService;

    public ActivityLogsController(IActivityLogService activityLogService)
    {
        _activityLogService = activityLogService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ActivityLogResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ActivityLogResponseDto>> Create([FromBody] CreateActivityLogRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var created = await _activityLogService.CreateActivityLogAsync(userId, request);
            return CreatedAtAction(nameof(GetAll), new { }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ActivityLogResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ActivityLogResponseDto>> Update(int id, [FromBody] UpdateActivityLogRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var updated = await _activityLogService.UpdateActivityLogAsync(userId, id, request);

            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ActivityLogResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<ActivityLogResponseDto>>> GetAll()
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var items = await _activityLogService.GetActivityLogsAsync(userId);
        return Ok(items);
    }

    [HttpGet("weekly-summary/{weekStartDate}")]
    [ProducesResponseType(typeof(WeeklyActivitySummaryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<WeeklyActivitySummaryResponseDto>> GetWeeklySummary(DateOnly weekStartDate)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var summary = await _activityLogService.GetWeeklySummaryAsync(userId, weekStartDate);
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