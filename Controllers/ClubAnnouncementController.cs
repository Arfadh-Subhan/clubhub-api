using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubAnnouncementController : ControllerBase
{
    private readonly IClubAnnouncementService _clubAnnouncementService;

    public ClubAnnouncementController(IClubAnnouncementService clubAnnouncementService)
    {
        _clubAnnouncementService = clubAnnouncementService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClubAnnouncementRequest request)
    {
        var result = await _clubAnnouncementService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not create the announcement!" });
        }

        return CreatedAtAction(nameof(GetById), new { clubAnnouncementId = result.ClubAnnouncementId }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllApproved([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            return BadRequest(new { message = "Page number must be at least 1." });
        }

        if (pageSize < 1)
        {
            return BadRequest(new { message = "Page size must be at least 1." });
        }

        var result = await _clubAnnouncementService.GetAllApprovedAsync(pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{clubAnnouncementId:int}")]
    public async Task<IActionResult> GetById(int clubAnnouncementId)
    {
        var result = await _clubAnnouncementService.GetByIdAsync(clubAnnouncementId);

        if (result == null)
        {
            return NotFound(new { message = "Announcement not found!" });
        }

        return Ok(result);
    }

    [HttpGet("club/{clubId:int}")]
    public async Task<IActionResult> GetByClub(int clubId)
    {
        var result = await _clubAnnouncementService.GetByClubAsync(clubId);

        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _clubAnnouncementService.GetPendingAsync();

        return Ok(result);
    }

    [HttpPut("{clubAnnouncementId:int}")]
    public async Task<IActionResult> Update(int clubAnnouncementId, [FromBody] UpdateClubAnnouncementRequest request)
    {
        var result = await _clubAnnouncementService.UpdateAsync(clubAnnouncementId, request);

        if (result == null)
        {
            return NotFound(new { message = "Announcement not found!" });
        }

        return Ok(new { message = "Announcement updated and sent for re-approval!", ClubAnnouncement = result });
    }

    [HttpPut("{clubAnnouncementId:int}/approve")]
    public async Task<IActionResult> Approve(int clubAnnouncementId, [FromBody] ReviewClubAnnouncementRequest request)
    {
        var result = await _clubAnnouncementService.ApproveAsync(clubAnnouncementId, request.ReviewedBy);

        if (result == null)
        {
            return NotFound(new { message = "Announcement not found!" });
        }

        return Ok(new { message = "Announcement approved successfully!", ClubAnnouncement = result });
    }

    [HttpPut("{clubAnnouncementId:int}/reject")]
    public async Task<IActionResult> Reject(int clubAnnouncementId, [FromBody] ReviewClubAnnouncementRequest request)
    {
        var reason = request.RejectionReason ?? "No reason provided.";

        var result = await _clubAnnouncementService.RejectAsync(clubAnnouncementId, request.ReviewedBy, reason);

        if (result == null)
        {
            return NotFound(new { message = "Announcement not found!" });
        }

        return Ok(new { message = "Announcement rejected!", ClubAnnouncement = result });
    }

    [HttpDelete("{clubAnnouncementId:int}")]
    public async Task<IActionResult> Delete(int clubAnnouncementId)
    {
        var result = await _clubAnnouncementService.DeleteAsync(clubAnnouncementId);

        if (!result)
        {
            return NotFound(new { message = "Announcement not found!" });
        }

        return Ok(new { message = "Announcement deleted successfully!" });
    }
}