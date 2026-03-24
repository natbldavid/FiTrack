using FiTrack.Api.Dtos.Users.Requests;
using FiTrack.Api.Dtos.Users.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FiTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserGoalsHistoryController : ControllerBase
{
    private readonly IUserGoalsHistoryService _userGoalsHistoryService;

    public UserGoalsHistoryController(IUserGoalsHistoryService userGoalsHistoryService)
    {
        _userGoalsHistoryService = userGoalsHistoryService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserGoalsHistoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<UserGoalsHistoryResponseDto>>> GetGoalsHistory()
    {
        var userId = GetAuthenticatedUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _userGoalsHistoryService.GetGoalsHistoryAsync(userId.Value);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserGoalsHistoryResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserGoalsHistoryResponseDto>> CreateGoalsHistory(
        [FromBody] CreateUserGoalsHistoryRequestDto request)
    {
        var userId = GetAuthenticatedUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        try
        {
            var created = await _userGoalsHistoryService.CreateGoalsHistoryAsync(userId.Value, request);
            return CreatedAtAction(nameof(GetGoalsHistory), new { }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UserGoalsHistoryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserGoalsHistoryResponseDto>> UpdateGoalsHistory(
        int id,
        [FromBody] UpdateUserGoalsHistoryRequestDto request)
    {
        var userId = GetAuthenticatedUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var updated = await _userGoalsHistoryService.UpdateGoalsHistoryAsync(userId.Value, id, request);

        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    private int? GetAuthenticatedUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return null;
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return null;
        }

        return userId;
    }
}