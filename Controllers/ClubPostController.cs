using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubPostController : ControllerBase
{
    private readonly IClubPostService _clubPostService;

    public ClubPostController(IClubPostService clubPostService)
    {
        _clubPostService = clubPostService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClubPostRequest request)
    {
        var result = await _clubPostService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not create the post!" });
        }

        return CreatedAtAction(nameof(GetById), new { clubPostId = result.ClubPostId }, result);
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

        var result = await _clubPostService.GetAllApprovedAsync(pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{clubPostId:int}")]
    public async Task<IActionResult> GetById(int clubPostId)
    {
        var result = await _clubPostService.GetByIdAsync(clubPostId);

        if (result == null)
        {
            return NotFound(new { message = "Post not found!" });
        }

        return Ok(result);
    }

    [HttpGet("club/{clubId:int}")]
    public async Task<IActionResult> GetByClub(int clubId)
    {
        var result = await _clubPostService.GetByClubAsync(clubId);

        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _clubPostService.GetPendingAsync();

        return Ok(result);
    }

    [HttpPut("{clubPostId:int}")]
    public async Task<IActionResult> Update(int clubPostId, [FromBody] UpdateClubPostRequest request)
    {
        var result = await _clubPostService.UpdateAsync(clubPostId, request);

        if (result == null)
        {
            return NotFound(new { message = "Post not found!" });
        }

        return Ok(new { message = "Post updated and sent for re-approval!", ClubPost = result });
    }

    [HttpPut("{clubPostId:int}/approve")]
    public async Task<IActionResult> Approve(int clubPostId, [FromBody] ReviewClubPostRequest request)
    {
        var result = await _clubPostService.ApproveAsync(clubPostId, request.ReviewedBy);

        if (result == null)
        {
            return NotFound(new { message = "Post not found!" });
        }

        return Ok(new { message = "Post approved successfully!", ClubPost = result });
    }

    [HttpPut("{clubPostId:int}/reject")]
    public async Task<IActionResult> Reject(int clubPostId, [FromBody] ReviewClubPostRequest request)
    {
        var reason = request.RejectionReason ?? "No reason provided.";

        var result = await _clubPostService.RejectAsync(clubPostId, request.ReviewedBy, reason);

        if (result == null)
        {
            return NotFound(new { message = "Post not found!" });
        }

        return Ok(new { message = "Post rejected!", ClubPost = result });
    }

    [HttpDelete("{clubPostId:int}")]
    public async Task<IActionResult> Delete(int clubPostId)
    {
        var result = await _clubPostService.DeleteAsync(clubPostId);

        if (!result)
        {
            return NotFound(new { message = "Post not found!" });
        }

        return Ok(new { message = "Post deleted successfully!" });
    }
}