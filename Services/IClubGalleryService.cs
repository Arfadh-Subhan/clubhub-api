using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IClubGalleryService
{
    Task<ClubGalleryResponse?> CreateAsync(CreateClubGalleryRequest request);
    Task<List<ClubGalleryResponse>?> GetByClubAsync(int clubId);
    Task<ClubGalleryResponse?> GetByIdAsync(int clubGalleryId);
    Task<ClubGalleryResponse?> UpdateAsync(int clubGalleryId, UpdateClubGalleryRequest request);
    Task<bool> DeleteAsync(int clubGalleryId);
}