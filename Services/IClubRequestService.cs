using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IClubRequestService
{
    Task<ClubRequestResponse?> SubmitAsync(SubmitClubRequest request);
    Task<List<ClubRequestResponse>?> GetAllAsync();
    Task<List<ClubRequestResponse>?> GetPendingAsync();
    Task<ClubRequestResponse?> ApproveAsync(int clubRequestId, int reviewedBy);
    Task<ClubRequestResponse?> RejectAsync(int clubRequestId, int reviewedBy, string reason);
}