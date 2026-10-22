using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IGalleryPhotoService
{
    Task<GalleryPhotoResponse?> CreateAsync(CreateGalleryPhotoRequest request);
    Task<List<GalleryPhotoResponse>?> GetByGalleryAsync(int clubGalleryId);
    Task<bool> DeleteAsync(int galleryPhotoId);
}