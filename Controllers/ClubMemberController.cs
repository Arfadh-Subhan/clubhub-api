using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubMemberController : ControllerBase
{
    private readonly IClubMemberService _clubMemberService;

    public ClubMemberController(IClubMemberService clubMemberService)
    {
        _clubMemberService = clubMemberService;
    }

    [HttpPost("join")]
    public async Task<IActionResult> Join([FromBody] JoinClubRequest request)
    {
        var result = await _clubMemberService.JoinAsync(request);

        if (result == null)
        {
            return Conflict(new { message = "You have already requested to join this club!" });
        }

        return Ok(new { message = "Join request submitted successfully!", ClubMember = result });
    }

    [HttpGet("pending/{clubId:int}")]
    public async Task<IActionResult> GetPendingByClub(int clubId)
    {
        var result = await _clubMemberService.GetPendingByClubAsync(clubId);

        return Ok(result);
    }

    [HttpPut("{clubMemberId:int}/approve")]
    public async Task<IActionResult> Approve(int clubMemberId, [FromBody] ReviewClubRequest request)
    {
        var result = await _clubMemberService.ApproveAsync(clubMemberId, request.ReviewedBy);

        if (result == null)
        {
            return NotFound(new { message = "Join request not found!" });
        }

        return Ok(new { message = "Member approved successfully!", ClubMember = result });
    }

    [HttpPut("{clubMemberId:int}/reject")]
    public async Task<IActionResult> Reject(int clubMemberId, [FromBody] ReviewClubRequest request)
    {
        var result = await _clubMemberService.RejectAsync(clubMemberId, request.ReviewedBy);

        if (result == null)
        {
            return NotFound(new { message = "Join request not found!" });
        }

        return Ok(new { message = "Member rejected successfully!", ClubMember = result });
    }

    [HttpGet("club/{clubId:int}")]
    public async Task<IActionResult> GetByClub(int clubId)
    {
        var result = await _clubMemberService.GetByClubAsync(clubId);

        return Ok(result);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var result = await _clubMemberService.GetByUserAsync(userId);

        return Ok(result);
    }

    [HttpPost("executive")]
    public async Task<IActionResult> CreateExecutive([FromBody] CreateExecutiveRequest request)
    {
        var result = await _clubMemberService.CreateExecutiveAsync(request);

        if (result == null)
        {
            return Conflict(new { message = "Email is already taken or the position for this club is already filled!" });
        }

        return Ok(new { message = "Executive account created successfully!", ClubMember = result });
    }

    [HttpPut("{clubMemberId:int}/position")]
    public async Task<IActionResult> UpdatePosition(int clubMemberId, [FromBody] UpdatePositionRequest request)
    {
        var result = await _clubMemberService.UpdatePositionAsync(clubMemberId, request.Position);

        if (result == null)
        {
            return NotFound(new { message = "Club member not found!" });
        }

        return Ok(new { message = "Position updated successfully!", ClubMember = result });
    }

    [HttpDelete("{clubMemberId:int}")]
    public async Task<IActionResult> Remove(int clubMemberId)
    {
        var result = await _clubMemberService.RemoveAsync(clubMemberId);

        if (!result)
        {
            return NotFound(new { message = "Club member not found!" });
        }

        return Ok(new { message = "Member removed successfully!" });
    }
}