using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReminderController : ControllerBase
{
    private readonly IReminderService _reminderService;

    public ReminderController(IReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReminderRequest request)
    {
        var result = await _reminderService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not create reminder. Event not found or reminder already exists!" });
        }

        return Ok(new { message = "Reminder created successfully!", Reminder = result });
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var result = await _reminderService.GetByUserAsync(userId);

        return Ok(result);
    }

    [HttpGet("user/{userId:int}/active")]
    public async Task<IActionResult> GetActiveByUser(int userId)
    {
        var result = await _reminderService.GetActiveByUserAsync(userId);

        return Ok(result);
    }

    [HttpDelete("{reminderId:int}")]
    public async Task<IActionResult> Delete(int reminderId)
    {
        var result = await _reminderService.DeleteAsync(reminderId);

        if (!result)
        {
            return NotFound(new { message = "Reminder not found!" });
        }

        return Ok(new { message = "Reminder deleted successfully!" });
    }
}