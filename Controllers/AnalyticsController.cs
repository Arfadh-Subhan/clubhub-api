using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await _analyticsService.GetSummaryAsync();
        return Ok(result);
    }

    [HttpGet("members-by-club")]
    public async Task<IActionResult> GetMembersByClub()
    {
        var result = await _analyticsService.GetMembersByClubAsync();
        return Ok(result);
    }

    [HttpGet("events-by-club")]
    public async Task<IActionResult> GetEventsByClub()
    {
        var result = await _analyticsService.GetEventsByClubAsync();
        return Ok(result);
    }
}