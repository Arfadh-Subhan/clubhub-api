using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IcbtAdminController : ControllerBase
{
    private readonly IIcbtAdminService _icbtAdminService;

    public IcbtAdminController(IIcbtAdminService icbtAdminService)
    {
        _icbtAdminService = icbtAdminService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            return BadRequest(new { message = "Page number must be at least 1." });
        }

        if (pageSize < 1)
        {
            return BadRequest(new { message = "Page size must be at least 1." });
        }

        var result = await _icbtAdminService.GetAllUsersAsync(pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("users/{userId:int}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var result = await _icbtAdminService.GetUserByIdAsync(userId);

        if (result == null)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(result);
    }

    [HttpPut("users/{userId:int}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] IcbtAdminUpdateUserRequest request)
    {
        var result = await _icbtAdminService.UpdateUserAsync(userId, request);

        if (result == null)
        {
            return Conflict(new { message = "User not found or email is already taken!" });
        }

        return Ok(new { message = "User updated successfully!", User = result });
    }

    [HttpPut("users/{userId:int}/password")]
    public async Task<IActionResult> ResetPassword(int userId, [FromBody] IcbtAdminResetPasswordRequest request)
    {
        var result = await _icbtAdminService.ResetPasswordAsync(userId, request);

        if (result == null)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(new { message = "Password reset successfully!", User = result });
    }

    [HttpPut("users/{userId:int}/suspend")]
    public async Task<IActionResult> SuspendUser(int userId)
    {
        var result = await _icbtAdminService.SuspendUserAsync(userId);

        if (result == null)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(new { message = "User suspended successfully!", User = result });
    }

    [HttpPut("users/{userId:int}/activate")]
    public async Task<IActionResult> ActivateUser(int userId)
    {
        var result = await _icbtAdminService.ActivateUserAsync(userId);

        if (result == null)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(new { message = "User activated successfully!", User = result });
    }

    [HttpDelete("users/{userId:int}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var result = await _icbtAdminService.DeleteUserAsync(userId);

        if (!result)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(new { message = "User and all related data deleted successfully!" });
    }
}