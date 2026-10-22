using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GalleryPhotoController : ControllerBase
{
    private readonly IGalleryPhotoService _galleryPhotoService;

    public GalleryPhotoController(IGalleryPhotoService galleryPhotoService)
    {
        _galleryPhotoService = galleryPhotoService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGalleryPhotoRequest request)
    {
        var result = await _galleryPhotoService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not upload the photo!" });
        }

        return Ok(new { message = "Photo uploaded successfully!", GalleryPhoto = result });
    }

    [HttpGet("gallery/{clubGalleryId:int}")]
    public async Task<IActionResult> GetByGallery(int clubGalleryId)
    {
        var result = await _galleryPhotoService.GetByGalleryAsync(clubGalleryId);

        return Ok(result);
    }

    [HttpDelete("{galleryPhotoId:int}")]
    public async Task<IActionResult> Delete(int galleryPhotoId)
    {
        var result = await _galleryPhotoService.DeleteAsync(galleryPhotoId);

        if (!result)
        {
            return NotFound(new { message = "Photo not found!" });
        }

        return Ok(new { message = "Photo deleted successfully!" });
    }
}