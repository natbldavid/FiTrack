using FiTrack.Api.Dtos.ExerciseCatalog.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExerciseCatalogController : ControllerBase
{
    private readonly IExerciseCatalogService _exerciseCatalogService;

    public ExerciseCatalogController(IExerciseCatalogService exerciseCatalogService)
    {
        _exerciseCatalogService = exerciseCatalogService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ExerciseCatalogResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<ExerciseCatalogResponseDto>>> GetAll()
    {
        var exercises = await _exerciseCatalogService.GetExercisesAsync();
        return Ok(exercises);
    }
}