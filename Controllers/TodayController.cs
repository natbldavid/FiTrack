using System.Security.Claims;
using FiTrack.Api.Dtos.Today.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodayController : ControllerBase
{
    private readonly ITodayService _todayService;

    public TodayController(ITodayService todayService)
    {
        _todayService = todayService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(TodayDashboardResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodayDashboardResponseDto>> Get()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized();
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        try
        {
            var dashboard = await _todayService.GetTodayDashboardAsync(userId, today);
            return Ok(dashboard);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}