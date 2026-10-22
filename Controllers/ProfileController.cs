using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetProfile(int userId)
    {
        var result = await _profileService.GetProfileAsync(userId);

        if (result == null)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(result);
    }

    [HttpPut("{userId:int}")]
    public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UpdateProfileRequest request)
    {
        var result = await _profileService.UpdateProfileAsync(userId, request);

        if (result == null)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(new { message = "Profile updated successfully!", User = result });
    }
}