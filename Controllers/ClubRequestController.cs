using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubRequestController : ControllerBase
{
    private readonly IClubRequestService _clubRequestService;

    public ClubRequestController(IClubRequestService clubRequestService)
    {
        _clubRequestService = clubRequestService;
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] SubmitClubRequest request)
    {
        var result = await _clubRequestService.SubmitAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not submit club request!" });
        }

        return Ok(new { message = "Club request submitted successfully!", ClubRequest = result });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _clubRequestService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _clubRequestService.GetPendingAsync();

        return Ok(result);
    }

    [HttpPut("{clubRequestId:int}/approve")]
    public async Task<IActionResult> Approve(int clubRequestId, [FromBody] ReviewClubRequest request)
    {
        var result = await _clubRequestService.ApproveAsync(clubRequestId, request.ReviewedBy);

        if (result == null)
        {
            return Conflict(new { message = "Club request not found or a club with the proposed name already exists!" });
        }

        return Ok(new { message = "Club request approved successfully!", ClubRequest = result });
    }

    [HttpPut("{clubRequestId:int}/reject")]
    public async Task<IActionResult> Reject(int clubRequestId, [FromBody] ReviewClubRequest request)
    {
        var reason = request.RejectionReason ?? "No reason provided.";

        var result = await _clubRequestService.RejectAsync(clubRequestId, request.ReviewedBy, reason);

        if (result == null)
        {
            return NotFound(new { message = "Club request not found!" });
        }

        return Ok(new { message = "Club request rejected!", ClubRequest = result });
    }
}