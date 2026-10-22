using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubMeetingController : ControllerBase
{
    private readonly IClubMeetingService _clubMeetingService;

    public ClubMeetingController(IClubMeetingService clubMeetingService)
    {
        _clubMeetingService = clubMeetingService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClubMeetingRequest request)
    {
        var result = await _clubMeetingService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not create the meeting!" });
        }

        return CreatedAtAction(nameof(GetById), new { clubMeetingId = result.ClubMeetingId }, result);
    }

    [HttpGet("{clubMeetingId:int}")]
    public async Task<IActionResult> GetById(int clubMeetingId)
    {
        var result = await _clubMeetingService.GetByIdAsync(clubMeetingId);

        if (result == null)
        {
            return NotFound(new { message = "Meeting not found!" });
        }

        return Ok(result);
    }

    [HttpGet("club/{clubId:int}")]
    public async Task<IActionResult> GetByClub(int clubId)
    {
        var result = await _clubMeetingService.GetByClubAsync(clubId);

        return Ok(result);
    }

    [HttpPut("{clubMeetingId:int}")]
    public async Task<IActionResult> Update(int clubMeetingId, [FromBody] UpdateClubMeetingRequest request)
    {
        var result = await _clubMeetingService.UpdateAsync(clubMeetingId, request);

        if (result == null)
        {
            return NotFound(new { message = "Meeting not found!" });
        }

        return Ok(new { message = "Meeting updated successfully!", ClubMeeting = result });
    }

    [HttpDelete("{clubMeetingId:int}")]
    public async Task<IActionResult> Delete(int clubMeetingId)
    {
        var result = await _clubMeetingService.DeleteAsync(clubMeetingId);

        if (!result)
        {
            return NotFound(new { message = "Meeting not found!" });
        }

        return Ok(new { message = "Meeting deleted successfully!" });
    }
}