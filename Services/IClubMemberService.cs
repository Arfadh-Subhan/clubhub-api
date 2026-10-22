using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IClubMemberService
{
    Task<ClubMemberResponse?> JoinAsync(JoinClubRequest request);
    Task<List<ClubMemberResponse>?> GetPendingByClubAsync(int clubId);
    Task<ClubMemberResponse?> ApproveAsync(int clubMemberId, int reviewedBy);
    Task<ClubMemberResponse?> RejectAsync(int clubMemberId, int reviewedBy);
    Task<List<ClubMemberResponse>?> GetByClubAsync(int clubId);
    Task<List<ClubMemberResponse>?> GetByUserAsync(int userId);
    Task<ClubMemberResponse?> CreateExecutiveAsync(CreateExecutiveRequest request);
    Task<ClubMemberResponse?> UpdatePositionAsync(int clubMemberId, string position);
    Task<bool> RemoveAsync(int clubMemberId);
}