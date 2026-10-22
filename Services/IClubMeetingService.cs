using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IClubMeetingService
{
    Task<ClubMeetingResponse?> CreateAsync(CreateClubMeetingRequest request);
    Task<List<ClubMeetingResponse>?> GetByClubAsync(int clubId);
    Task<ClubMeetingResponse?> GetByIdAsync(int clubMeetingId);
    Task<ClubMeetingResponse?> UpdateAsync(int clubMeetingId, UpdateClubMeetingRequest request);
    Task<bool> DeleteAsync(int clubMeetingId);
}