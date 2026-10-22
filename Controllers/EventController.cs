using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        var result = await _eventService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not create the event!" });
        }

        return CreatedAtAction(nameof(GetById), new { eventId = result.EventId }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetUpcoming([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            return BadRequest(new { message = "Page number must be at least 1." });
        }

        if (pageSize < 1)
        {
            return BadRequest(new { message = "Page size must be at least 1." });
        }

        var result = await _eventService.GetUpcomingAsync(pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{eventId:int}")]
    public async Task<IActionResult> GetById(int eventId)
    {
        var result = await _eventService.GetByIdAsync(eventId);

        if (result == null)
        {
            return NotFound(new { message = "Event not found!" });
        }

        return Ok(result);
    }

    [HttpGet("club/{clubId:int}")]
    public async Task<IActionResult> GetByClub(int clubId)
    {
        var result = await _eventService.GetByClubAsync(clubId);

        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _eventService.GetPendingAsync();

        return Ok(result);
    }

    [HttpPut("{eventId:int}")]
    public async Task<IActionResult> Update(int eventId, [FromBody] UpdateEventRequest request)
    {
        var result = await _eventService.UpdateAsync(eventId, request);

        if (result == null)
        {
            return NotFound(new { message = "Event not found!" });
        }

        return Ok(new { message = "Event updated and sent for re-approval!", Event = result });
    }

    [HttpPut("{eventId:int}/approve")]
    public async Task<IActionResult> Approve(int eventId, [FromBody] ReviewEventRequest request)
    {
        var result = await _eventService.ApproveAsync(eventId, request.ReviewedBy);

        if (result == null)
        {
            return NotFound(new { message = "Event not found!" });
        }

        return Ok(new { message = "Event approved successfully!", Event = result });
    }

    [HttpPut("{eventId:int}/reject")]
    public async Task<IActionResult> Reject(int eventId, [FromBody] ReviewEventRequest request)
    {
        var reason = request.RejectionReason ?? "No reason provided.";

        var result = await _eventService.RejectAsync(eventId, request.ReviewedBy, reason);

        if (result == null)
        {
            return NotFound(new { message = "Event not found!" });
        }

        return Ok(new { message = "Event rejected!", Event = result });
    }

    [HttpDelete("{eventId:int}")]
    public async Task<IActionResult> Delete(int eventId)
    {
        var result = await _eventService.DeleteAsync(eventId);

        if (!result)
        {
            return NotFound(new { message = "Event not found!" });
        }

        return Ok(new { message = "Event deleted successfully!" });
    }
}