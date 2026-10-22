using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IClubProductService
{
    Task<ClubProductResponse?> CreateAsync(CreateClubProductRequest request);
    Task<PageResult<ClubProductResponse>?> GetAllApprovedAsync(int pageNumber, int pageSize);
    Task<ClubProductResponse?> GetByIdAsync(int clubProductId);
    Task<List<ClubProductResponse>?> GetByClubAsync(int clubId);
    Task<List<ClubProductResponse>?> GetPendingAsync();
    Task<ClubProductResponse?> UpdateAsync(int clubProductId, UpdateClubProductRequest request);
    Task<ClubProductResponse?> ApproveAsync(int clubProductId, int reviewedBy);
    Task<ClubProductResponse?> RejectAsync(int clubProductId, int reviewedBy, string reason);
    Task<bool> DeleteAsync(int clubProductId);
}
