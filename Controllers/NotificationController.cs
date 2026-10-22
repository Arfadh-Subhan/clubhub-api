using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request)
    {
        var result = await _notificationService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not create notification!" });
        }

        return Ok(new { message = "Notification created successfully!", Notification = result });
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var result = await _notificationService.GetByUserAsync(userId);

        return Ok(result);
    }

    [HttpGet("user/{userId:int}/unread")]
    public async Task<IActionResult> GetUnreadByUser(int userId)
    {
        var result = await _notificationService.GetUnreadByUserAsync(userId);

        return Ok(result);
    }

    [HttpPut("{notificationId:int}/read")]
    public async Task<IActionResult> MarkAsRead(int notificationId)
    {
        var result = await _notificationService.MarkAsReadAsync(notificationId);

        if (result == null)
        {
            return NotFound(new { message = "Notification not found!" });
        }

        return Ok(new { message = "Notification marked as read!", Notification = result });
    }

    [HttpPut("user/{userId:int}/read-all")]
    public async Task<IActionResult> MarkAllAsRead(int userId)
    {
        var result = await _notificationService.MarkAllAsReadAsync(userId);

        if (!result)
        {
            return BadRequest(new { message = "Could not mark notifications as read!" });
        }

        return Ok(new { message = "All notifications marked as read!" });
    }

    [HttpDelete("{notificationId:int}")]
    public async Task<IActionResult> Delete(int notificationId)
    {
        var result = await _notificationService.DeleteAsync(notificationId);

        if (!result)
        {
            return NotFound(new { message = "Notification not found!" });
        }

        return Ok(new { message = "Notification deleted successfully!" });
    }
}