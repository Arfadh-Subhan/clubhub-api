using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubProductController : ControllerBase
{
    private readonly IClubProductService _clubProductService;

    public ClubProductController(IClubProductService clubProductService)
    {
        _clubProductService = clubProductService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClubProductRequest request)
    {
        var result = await _clubProductService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not create the product!" });
        }

        return CreatedAtAction(nameof(GetById), new { clubProductId = result.ClubProductId }, result);
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

        var result = await _clubProductService.GetAllApprovedAsync(pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{clubProductId:int}")]
    public async Task<IActionResult> GetById(int clubProductId)
    {
        var result = await _clubProductService.GetByIdAsync(clubProductId);

        if (result == null)
        {
            return NotFound(new { message = "Product not found!" });
        }

        return Ok(result);
    }

    [HttpGet("club/{clubId:int}")]
    public async Task<IActionResult> GetByClub(int clubId)
    {
        var result = await _clubProductService.GetByClubAsync(clubId);

        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _clubProductService.GetPendingAsync();

        return Ok(result);
    }

    [HttpPut("{clubProductId:int}")]
    public async Task<IActionResult> Update(int clubProductId, [FromBody] UpdateClubProductRequest request)
    {
        var result = await _clubProductService.UpdateAsync(clubProductId, request);

        if (result == null)
        {
            return NotFound(new { message = "Product not found!" });
        }

        return Ok(new { message = "Product updated and sent for re-approval!", ClubProduct = result });
    }

    [HttpPut("{clubProductId:int}/approve")]
    public async Task<IActionResult> Approve(int clubProductId, [FromBody] ReviewClubProductRequest request)
    {
        var result = await _clubProductService.ApproveAsync(clubProductId, request.ReviewedBy);

        if (result == null)
        {
            return NotFound(new { message = "Product not found!" });
        }

        return Ok(new { message = "Product approved successfully!", ClubProduct = result });
    }

    [HttpPut("{clubProductId:int}/reject")]
    public async Task<IActionResult> Reject(int clubProductId, [FromBody] ReviewClubProductRequest request)
    {
        var reason = request.RejectionReason ?? "No reason provided.";

        var result = await _clubProductService.RejectAsync(clubProductId, request.ReviewedBy, reason);

        if (result == null)
        {
            return NotFound(new { message = "Product not found!" });
        }

        return Ok(new { message = "Product rejected!", ClubProduct = result });
    }

    [HttpDelete("{clubProductId:int}")]
    public async Task<IActionResult> Delete(int clubProductId)
    {
        var result = await _clubProductService.DeleteAsync(clubProductId);

        if (!result)
        {
            return NotFound(new { message = "Product not found!" });
        }

        return Ok(new { message = "Product deleted successfully!" });
    }
}