using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ClubGalleryService : IClubGalleryService
{
    private readonly AppDbContext _context;

    public ClubGalleryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClubGalleryResponse?> CreateAsync(CreateClubGalleryRequest request)
    {
        var clubGallery = new ClubGallery
        {
            ClubId = request.ClubId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            CoverPhoto = request.CoverPhoto,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.ClubGalleries.Add(clubGallery);
        await _context.SaveChangesAsync();

        return MapToResponse(clubGallery);
    }

    public async Task<List<ClubGalleryResponse>?> GetByClubAsync(int clubId)
    {
        return await _context.ClubGalleries
            .AsNoTracking()
            .Where(clubGallery => clubGallery.ClubId == clubId && clubGallery.IsActive)
            .OrderByDescending(clubGallery => clubGallery.CreatedAt)
            .Select(clubGallery => new ClubGalleryResponse
            {
                ClubGalleryId = clubGallery.ClubGalleryId,
                ClubId = clubGallery.ClubId,
                Title = clubGallery.Title,
                Description = clubGallery.Description,
                CoverPhoto = clubGallery.CoverPhoto,
                CreatedBy = clubGallery.CreatedBy,
                CreatedAt = clubGallery.CreatedAt,
                UpdatedAt = clubGallery.UpdatedAt,
                IsActive = clubGallery.IsActive
            })
            .ToListAsync();
    }

    public async Task<ClubGalleryResponse?> GetByIdAsync(int clubGalleryId)
    {
        return await _context.ClubGalleries
            .AsNoTracking()
            .Where(clubGallery => clubGallery.ClubGalleryId == clubGalleryId)
            .Select(clubGallery => new ClubGalleryResponse
            {
                ClubGalleryId = clubGallery.ClubGalleryId,
                ClubId = clubGallery.ClubId,
                Title = clubGallery.Title,
                Description = clubGallery.Description,
                CoverPhoto = clubGallery.CoverPhoto,
                CreatedBy = clubGallery.CreatedBy,
                CreatedAt = clubGallery.CreatedAt,
                UpdatedAt = clubGallery.UpdatedAt,
                IsActive = clubGallery.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ClubGalleryResponse?> UpdateAsync(int clubGalleryId, UpdateClubGalleryRequest request)
    {
        var clubGallery = await _context.ClubGalleries.FindAsync(clubGalleryId);

        if (clubGallery == null)
        {
            return null;
        }

        clubGallery.Title = request.Title.Trim();
        clubGallery.Description = request.Description.Trim();
        clubGallery.CoverPhoto = request.CoverPhoto;
        clubGallery.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(clubGallery);
    }

    public async Task<bool> DeleteAsync(int clubGalleryId)
    {
        var clubGallery = await _context.ClubGalleries.FindAsync(clubGalleryId);

        if (clubGallery == null)
        {
            return false;
        }

        clubGallery.IsActive = false;
        clubGallery.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private static ClubGalleryResponse MapToResponse(ClubGallery clubGallery)
    {
        return new ClubGalleryResponse
        {
            ClubGalleryId = clubGallery.ClubGalleryId,
            ClubId = clubGallery.ClubId,
            Title = clubGallery.Title,
            Description = clubGallery.Description,
            CoverPhoto = clubGallery.CoverPhoto,
            CreatedBy = clubGallery.CreatedBy,
            CreatedAt = clubGallery.CreatedAt,
            UpdatedAt = clubGallery.UpdatedAt,
            IsActive = clubGallery.IsActive
        };
    }
}