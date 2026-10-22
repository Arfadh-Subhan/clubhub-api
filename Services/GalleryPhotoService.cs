using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class GalleryPhotoService : IGalleryPhotoService
{
    private readonly AppDbContext _context;

    public GalleryPhotoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GalleryPhotoResponse?> CreateAsync(CreateGalleryPhotoRequest request)
    {
        var galleryPhoto = new GalleryPhoto
        {
            ClubGalleryId = request.ClubGalleryId,
            Photo = request.Photo,
            Caption = request.Caption,
            UploadedAt = DateTime.UtcNow
        };

        _context.GalleryPhotos.Add(galleryPhoto);
        await _context.SaveChangesAsync();

        return MapToResponse(galleryPhoto);
    }

    public async Task<List<GalleryPhotoResponse>?> GetByGalleryAsync(int clubGalleryId)
    {
        return await _context.GalleryPhotos
            .AsNoTracking()
            .Where(galleryPhoto => galleryPhoto.ClubGalleryId == clubGalleryId)
            .OrderByDescending(galleryPhoto => galleryPhoto.UploadedAt)
            .Select(galleryPhoto => new GalleryPhotoResponse
            {
                GalleryPhotoId = galleryPhoto.GalleryPhotoId,
                ClubGalleryId = galleryPhoto.ClubGalleryId,
                Photo = galleryPhoto.Photo,
                Caption = galleryPhoto.Caption,
                UploadedAt = galleryPhoto.UploadedAt
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int galleryPhotoId)
    {
        var galleryPhoto = await _context.GalleryPhotos.FindAsync(galleryPhotoId);

        if (galleryPhoto == null)
        {
            return false;
        }

        _context.GalleryPhotos.Remove(galleryPhoto);
        await _context.SaveChangesAsync();

        return true;
    }

    private static GalleryPhotoResponse MapToResponse(GalleryPhoto galleryPhoto)
    {
        return new GalleryPhotoResponse
        {
            GalleryPhotoId = galleryPhoto.GalleryPhotoId,
            ClubGalleryId = galleryPhoto.ClubGalleryId,
            Photo = galleryPhoto.Photo,
            Caption = galleryPhoto.Caption,
            UploadedAt = galleryPhoto.UploadedAt
        };
    }
}