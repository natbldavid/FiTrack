using System.Security.Claims;
using FiTrack.Api.Dtos.Meals.Requests;
using FiTrack.Api.Dtos.Meals.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MealsController : ControllerBase
{
    private readonly IMealService _mealService;

    public MealsController(IMealService mealService)
    {
        _mealService = mealService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(MealDetailResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MealDetailResponseDto>> Create([FromBody] CreateMealRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var createdMeal = await _mealService.CreateMealAsync(userId, request);

            return CreatedAtAction(nameof(GetById), new { id = createdMeal.Id }, createdMeal);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MealDetailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MealDetailResponseDto>> Update(int id, [FromBody] UpdateMealRequestDto request)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var updatedMeal = await _mealService.UpdateMealAsync(userId, id, request);

            if (updatedMeal is null)
            {
                return NotFound();
            }

            return Ok(updatedMeal);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<MealSummaryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<MealSummaryResponseDto>>> GetAll()
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var meals = await _mealService.GetMealsAsync(userId);
        return Ok(meals);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MealDetailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MealDetailResponseDto>> GetById(int id)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var meal = await _mealService.GetMealByIdAsync(userId, id);

        if (meal is null)
        {
            return NotFound();
        }

        return Ok(meal);
    }

    private bool TryGetUserId(out int userId)
    {
        userId = 0;

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return !string.IsNullOrWhiteSpace(userIdClaim)
            && int.TryParse(userIdClaim, out userId);
    }
}