using System.Security.Claims;
using FiTrack.Api.Dtos.WorkoutSessions.Requests;
using FiTrack.Api.Dtos.WorkoutSessions.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkoutSessionsController : ControllerBase
{
    private readonly IWorkoutSessionService _workoutSessionService;

    public WorkoutSessionsController(IWorkoutSessionService workoutSessionService)
    {
        _workoutSessionService = workoutSessionService;
    }

    [HttpPost("start")]
    [ProducesResponseType(typeof(WorkoutSessionResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutSessionResponseDto>> Start([FromBody] StartWorkoutSessionRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var created = await _workoutSessionService.StartWorkoutSessionAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<WorkoutSessionSummaryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<WorkoutSessionSummaryResponseDto>>> GetAll()
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var items = await _workoutSessionService.GetWorkoutSessionsAsync(userId);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WorkoutSessionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutSessionResponseDto>> GetById(int id)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var item = await _workoutSessionService.GetWorkoutSessionByIdAsync(userId, id);

        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPut("{id:int}/sets")]
    [ProducesResponseType(typeof(WorkoutSessionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutSessionResponseDto>> UpsertSet(int id, [FromBody] UpdateWorkoutSetLogRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var updated = await _workoutSessionService.UpsertWorkoutSetLogAsync(userId, id, request);

            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
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

    [HttpPost("{id:int}/complete")]
    [ProducesResponseType(typeof(WorkoutSessionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutSessionResponseDto>> Complete(int id, [FromBody] CompleteWorkoutSessionRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var completed = await _workoutSessionService.CompleteWorkoutSessionAsync(userId, id, request);

            if (completed is null)
            {
                return NotFound();
            }

            return Ok(completed);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private bool TryGetUserId(out int userId)
    {
        userId = 0;

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return !string.IsNullOrWhiteSpace(userIdClaim)
            && int.TryParse(userIdClaim, out userId);
    }
}