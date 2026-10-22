using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SiteAdminController : ControllerBase
{
    private readonly IIcbtAdminService _icbtAdminService;

    public SiteAdminController(IIcbtAdminService icbtAdminService)
    {
        _icbtAdminService = icbtAdminService;
    }

    [HttpGet("accounts")]
    public async Task<IActionResult> GetAllAccounts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
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

    [HttpGet("accounts/{userId:int}")]
    public async Task<IActionResult> GetAccountById(int userId)
    {
        var result = await _icbtAdminService.GetUserByIdAsync(userId);

        if (result == null)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(result);
    }

    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount([FromBody] SiteAdminCreateUserRequest request)
    {
        if (request.Role == "Executive")
        {
            return Conflict(new { message = "Executives must be created by ICBT Admin!" });
        }

        if (request.Role != "Student" && request.Role != "IcbtAdmin" && request.Role != "SuperAdmin")
        {
            return BadRequest(new { message = "Role must be Student, IcbtAdmin, or SuperAdmin." });
        }

        var result = await _icbtAdminService.CreateUserAsync(request);

        if (result == null)
        {
            return Conflict(new { message = "Email or Student ID is already taken!" });
        }

        return Ok(new { message = "Account created successfully!", User = result });
    }

    [HttpPut("accounts/{userId:int}")]
    public async Task<IActionResult> UpdateAccount(int userId, [FromBody] IcbtAdminUpdateUserRequest request)
    {
        var result = await _icbtAdminService.UpdateUserAsync(userId, request);

        if (result == null)
        {
            return Conflict(new { message = "User not found or email is already taken!" });
        }

        return Ok(new { message = "Account updated successfully!", User = result });
    }

    [HttpPut("accounts/{userId:int}/password")]
    public async Task<IActionResult> ResetPassword(int userId, [FromBody] IcbtAdminResetPasswordRequest request)
    {
        var result = await _icbtAdminService.ResetPasswordAsync(userId, request);

        if (result == null)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(new { message = "Password reset successfully!", User = result });
    }

    [HttpPut("accounts/{userId:int}/suspend")]
    public async Task<IActionResult> SuspendAccount(int userId)
    {
        var result = await _icbtAdminService.SuspendUserAsync(userId);

        if (result == null)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(new { message = "Account suspended successfully!", User = result });
    }

    [HttpPut("accounts/{userId:int}/activate")]
    public async Task<IActionResult> ActivateAccount(int userId)
    {
        var result = await _icbtAdminService.ActivateUserAsync(userId);

        if (result == null)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(new { message = "Account activated successfully!", User = result });
    }

    [HttpDelete("accounts/{userId:int}")]
    public async Task<IActionResult> DeleteAccount(int userId)
    {
        var result = await _icbtAdminService.DeleteUserAsync(userId);

        if (!result)
        {
            return NotFound(new { message = "User not found!" });
        }

        return Ok(new { message = "Account and all related data deleted successfully!" });
    }
}