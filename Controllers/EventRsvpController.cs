using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventRsvpController : ControllerBase
{
    private readonly IEventRsvpService _eventRsvpService;

    public EventRsvpController(IEventRsvpService eventRsvpService)
    {
        _eventRsvpService = eventRsvpService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRsvpRequest request)
    {
        var result = await _eventRsvpService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not RSVP. Event not available, already RSVP'd, or full!" });
        }

        return Ok(new { message = "RSVP successful!", EventRsvp = result });
    }

    [HttpGet("event/{eventId:int}")]
    public async Task<IActionResult> GetByEvent(int eventId)
    {
        var result = await _eventRsvpService.GetByEventAsync(eventId);

        return Ok(result);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var result = await _eventRsvpService.GetByUserAsync(userId);

        return Ok(result);
    }

    [HttpDelete("{eventRsvpId:int}")]
    public async Task<IActionResult> Delete(int eventRsvpId)
    {
        var result = await _eventRsvpService.DeleteAsync(eventRsvpId);

        if (!result)
        {
            return NotFound(new { message = "RSVP not found!" });
        }

        return Ok(new { message = "RSVP cancelled successfully!" });
    }
}