using ClubHub.DTOs;

namespace ClubHub.Services;

public interface ILikeService
{
    Task<LikeResponse?> ToggleAsync(ToggleLikeRequest request);
    Task<LikeResponse?> GetByEventAsync(int eventId);
}