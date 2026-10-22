using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivityLogController : ControllerBase
{
    private readonly IActivityLogService _activityLogService;

    public ActivityLogController(IActivityLogService activityLogService)
    {
        _activityLogService = activityLogService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateActivityLogRequest request)
    {
        var result = await _activityLogService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not create the log entry!" });
        }

        return Ok(new { message = "Log entry created successfully!", ActivityLog = result });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _activityLogService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("type/{type}")]
    public async Task<IActionResult> GetByType(string type)
    {
        var result = await _activityLogService.GetByTypeAsync(type);

        return Ok(result);
    }

    [HttpDelete("{activityLogId:int}")]
    public async Task<IActionResult> Delete(int activityLogId)
    {
        var result = await _activityLogService.DeleteAsync(activityLogId);

        if (!result)
        {
            return NotFound(new { message = "Log entry not found!" });
        }

        return Ok(new { message = "Log entry deleted successfully!" });
    }
}