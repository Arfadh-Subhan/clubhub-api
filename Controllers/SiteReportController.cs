using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SiteReportController : ControllerBase
{
    private readonly ISiteReportService _siteReportService;

    public SiteReportController(ISiteReportService siteReportService)
    {
        _siteReportService = siteReportService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSiteReportRequest request)
    {
        var result = await _siteReportService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not submit the report!" });
        }

        return Ok(new { message = "Report submitted successfully!", SiteReport = result });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _siteReportService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{siteReportId:int}")]
    public async Task<IActionResult> GetById(int siteReportId)
    {
        var result = await _siteReportService.GetByIdAsync(siteReportId);

        if (result == null)
        {
            return NotFound(new { message = "Report not found!" });
        }

        return Ok(result);
    }

    [HttpPut("{siteReportId:int}/status")]
    public async Task<IActionResult> UpdateStatus(int siteReportId, [FromBody] UpdateSiteReportStatusRequest request)
    {
        var result = await _siteReportService.UpdateStatusAsync(siteReportId, request.Status);

        if (result == null)
        {
            return NotFound(new { message = "Report not found!" });
        }

        return Ok(new { message = "Report status updated!", SiteReport = result });
    }

    [HttpDelete("{siteReportId:int}")]
    public async Task<IActionResult> Delete(int siteReportId)
    {
        var result = await _siteReportService.DeleteAsync(siteReportId);

        if (!result)
        {
            return NotFound(new { message = "Report not found!" });
        }

        return Ok(new { message = "Report deleted successfully!" });
    }
}