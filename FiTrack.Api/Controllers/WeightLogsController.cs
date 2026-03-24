using System.Security.Claims;
using FiTrack.Api.Dtos.WeightLogs.Requests;
using FiTrack.Api.Dtos.WeightLogs.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeightLogsController : ControllerBase
{
    private readonly IWeightLogService _weightLogService;

    public WeightLogsController(IWeightLogService weightLogService)
    {
        _weightLogService = weightLogService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(WeightLogResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeightLogResponseDto>> Create([FromBody] CreateWeightLogRequestDto request)
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

        try
        {
            var createdWeightLog = await _weightLogService.CreateWeightLogAsync(userId, request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdWeightLog.Id },
                createdWeightLog);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WeightLogResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        return NotFound();
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(WeightLogResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeightLogResponseDto>> Update(int id, [FromBody] UpdateWeightLogRequestDto request)
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

        var updatedWeightLog = await _weightLogService.UpdateWeightLogAsync(userId, id, request);

        if (updatedWeightLog is null)
        {
            return NotFound();
        }

        return Ok(updatedWeightLog);
    }

    [HttpGet("trend")]
    [ProducesResponseType(typeof(List<WeightTrendPointDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<WeightTrendPointDto>>> GetTrend()
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

        var trend = await _weightLogService.GetWeightTrendAsync(userId);

        return Ok(trend);
    }
}