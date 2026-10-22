using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LikeController : ControllerBase
{
    private readonly ILikeService _likeService;

    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [HttpPost("toggle")]
    public async Task<IActionResult> Toggle([FromBody] ToggleLikeRequest request)
    {
        var result = await _likeService.ToggleAsync(request);

        if (result == null)
        {
            return NotFound(new { message = "Event not found!" });
        }

        return Ok(new { message = "Like toggled successfully!", Like = result });
    }

    [HttpGet("event/{eventId:int}")]
    public async Task<IActionResult> GetByEvent(int eventId)
    {
        var result = await _likeService.GetByEventAsync(eventId);

        if (result == null)
        {
            return NotFound(new { message = "Event not found!" });
        }

        return Ok(result);
    }
}