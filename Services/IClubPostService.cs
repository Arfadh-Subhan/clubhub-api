using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IClubPostService
{
    Task<ClubPostResponse?> CreateAsync(CreateClubPostRequest request);
    Task<PageResult<ClubPostResponse>?> GetAllApprovedAsync(int pageNumber, int pageSize);
    Task<ClubPostResponse?> GetByIdAsync(int clubPostId);
    Task<List<ClubPostResponse>?> GetByClubAsync(int clubId);
    Task<List<ClubPostResponse>?> GetPendingAsync();
    Task<ClubPostResponse?> UpdateAsync(int clubPostId, UpdateClubPostRequest request);
    Task<ClubPostResponse?> ApproveAsync(int clubPostId, int reviewedBy);
    Task<ClubPostResponse?> RejectAsync(int clubPostId, int reviewedBy, string reason);
    Task<bool> DeleteAsync(int clubPostId);
}