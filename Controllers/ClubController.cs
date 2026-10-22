using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubController : ControllerBase
{
    private readonly IClubService _clubService;

    public ClubController(IClubService clubService)
    {
        _clubService = clubService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClubRequest request)
    {
        var result = await _clubService.CreateAsync(request);

        if (result == null)
        {
            return Conflict(new { message = "Club name is already taken!" });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.ClubId }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            return BadRequest(new { message = "Page number must be at least 1." });
        }

        if (pageSize < 1)
        {
            return BadRequest(new { message = "Page size must be at least 1." });
        }

        var result = await _clubService.GetAllAsync(pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _clubService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new { message = "Club not found!" });
        }

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClubRequest request)
    {
        var result = await _clubService.UpdateAsync(id, request);

        if (result == null)
        {
            return NotFound(new { message = "Club not found or name already taken!" });
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _clubService.DeleteAsync(id);

        if (!result)
        {
            return NotFound(new { message = "Club not found!" });
        }

        return Ok(new { message = "Club deleted successfully!" });
    }
}