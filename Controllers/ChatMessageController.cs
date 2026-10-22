using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatMessageController : ControllerBase
{
    private readonly IChatMessageService _chatMessageService;

    public ChatMessageController(IChatMessageService chatMessageService)
    {
        _chatMessageService = chatMessageService;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] CreateChatMessageRequest request)
    {
        var result = await _chatMessageService.SendAsync(request);

        if (result == null)
        {
            return StatusCode(403, new { message = "You are not an approved member of this club!" });
        }

        return Ok(new { message = "Message sent successfully!", ChatMessage = result });
    }

    [HttpGet("club/{clubId:int}")]
    public async Task<IActionResult> GetByClub(int clubId)
    {
        var result = await _chatMessageService.GetByClubAsync(clubId);

        return Ok(result);
    }

    [HttpGet("club/{clubId:int}/after/{chatMessageId:int}")]
    public async Task<IActionResult> GetAfterId(int clubId, int chatMessageId)
    {
        var result = await _chatMessageService.GetAfterIdAsync(clubId, chatMessageId);

        return Ok(result);
    }

    [HttpDelete("{chatMessageId:int}")]
    public async Task<IActionResult> Delete(int chatMessageId)
    {
        var result = await _chatMessageService.DeleteAsync(chatMessageId);

        if (!result)
        {
            return NotFound(new { message = "Message not found!" });
        }

        return Ok(new { message = "Message deleted successfully!" });
    }
}