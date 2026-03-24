using FiTrack.Api.Dtos.ActivityTypes.Responses;
using FiTrack.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivityTypesController : ControllerBase
{
    private readonly IActivityTypeService _activityTypeService;

    public ActivityTypesController(IActivityTypeService activityTypeService)
    {
        _activityTypeService = activityTypeService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ActivityTypeResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<ActivityTypeResponseDto>>> GetAll()
    {
        var items = await _activityTypeService.GetActivityTypesAsync();
        return Ok(items);
    }
}