using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IClubAnnouncementService
{
    Task<ClubAnnouncementResponse?> CreateAsync(CreateClubAnnouncementRequest request);
    Task<PageResult<ClubAnnouncementResponse>?> GetAllApprovedAsync(int pageNumber, int pageSize);
    Task<ClubAnnouncementResponse?> GetByIdAsync(int clubAnnouncementId);
    Task<List<ClubAnnouncementResponse>?> GetByClubAsync(int clubId);
    Task<List<ClubAnnouncementResponse>?> GetPendingAsync();
    Task<ClubAnnouncementResponse?> UpdateAsync(int clubAnnouncementId, UpdateClubAnnouncementRequest request);
    Task<ClubAnnouncementResponse?> ApproveAsync(int clubAnnouncementId, int reviewedBy);
    Task<ClubAnnouncementResponse?> RejectAsync(int clubAnnouncementId, int reviewedBy, string reason);
    Task<bool> DeleteAsync(int clubAnnouncementId);
}