using System.Security.Claims;
using FiTrack.Api.Dtos.WorkoutDays.Requests;
using FiTrack.Api.Dtos.WorkoutDays.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkoutDaysController : ControllerBase
{
    private readonly IWorkoutDayService _workoutDayService;

    public WorkoutDaysController(IWorkoutDayService workoutDayService)
    {
        _workoutDayService = workoutDayService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(WorkoutDayDetailResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutDayDetailResponseDto>> Create([FromBody] CreateWorkoutDayRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var created = await _workoutDayService.CreateWorkoutDayAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
    [ProducesResponseType(typeof(WorkoutDayDetailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutDayDetailResponseDto>> Update(int id, [FromBody] UpdateWorkoutDayRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var updated = await _workoutDayService.UpdateWorkoutDayAsync(userId, id, request);

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

    [HttpGet]
    [ProducesResponseType(typeof(List<WorkoutDaySummaryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<WorkoutDaySummaryResponseDto>>> GetAll()
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var items = await _workoutDayService.GetWorkoutDaysAsync(userId);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WorkoutDayDetailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutDayDetailResponseDto>> GetById(int id)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var item = await _workoutDayService.GetWorkoutDayByIdAsync(userId, id);

        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    private bool TryGetUserId(out int userId)
    {
        userId = 0;

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return !string.IsNullOrWhiteSpace(userIdClaim)
            && int.TryParse(userIdClaim, out userId);
    }
}