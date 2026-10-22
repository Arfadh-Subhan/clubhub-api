using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubGalleryController : ControllerBase
{
    private readonly IClubGalleryService _clubGalleryService;

    public ClubGalleryController(IClubGalleryService clubGalleryService)
    {
        _clubGalleryService = clubGalleryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClubGalleryRequest request)
    {
        var result = await _clubGalleryService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not create the gallery!" });
        }

        return CreatedAtAction(nameof(GetById), new { clubGalleryId = result.ClubGalleryId }, result);
    }

    [HttpGet("{clubGalleryId:int}")]
    public async Task<IActionResult> GetById(int clubGalleryId)
    {
        var result = await _clubGalleryService.GetByIdAsync(clubGalleryId);

        if (result == null)
        {
            return NotFound(new { message = "Gallery not found!" });
        }

        return Ok(result);
    }

    [HttpGet("club/{clubId:int}")]
    public async Task<IActionResult> GetByClub(int clubId)
    {
        var result = await _clubGalleryService.GetByClubAsync(clubId);

        return Ok(result);
    }

    [HttpPut("{clubGalleryId:int}")]
    public async Task<IActionResult> Update(int clubGalleryId, [FromBody] UpdateClubGalleryRequest request)
    {
        var result = await _clubGalleryService.UpdateAsync(clubGalleryId, request);

        if (result == null)
        {
            return NotFound(new { message = "Gallery not found!" });
        }

        return Ok(new { message = "Gallery updated successfully!", ClubGallery = result });
    }

    [HttpDelete("{clubGalleryId:int}")]
    public async Task<IActionResult> Delete(int clubGalleryId)
    {
        var result = await _clubGalleryService.DeleteAsync(clubGalleryId);

        if (!result)
        {
            return NotFound(new { message = "Gallery not found!" });
        }

        return Ok(new { message = "Gallery deleted successfully!" });
    }
}