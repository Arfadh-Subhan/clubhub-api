using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IEventService
{
    Task<EventResponse?> CreateAsync(CreateEventRequest request);
    Task<PageResult<EventResponse>?> GetUpcomingAsync(int pageNumber, int pageSize);
    Task<List<EventResponse>?> GetByClubAsync(int clubId);
    Task<EventResponse?> GetByIdAsync(int eventId);
    Task<List<EventResponse>?> GetPendingAsync();
    Task<EventResponse?> UpdateAsync(int eventId, UpdateEventRequest request);
    Task<EventResponse?> ApproveAsync(int eventId, int reviewedBy);
    Task<EventResponse?> RejectAsync(int eventId, int reviewedBy, string reason);
    Task<bool> DeleteAsync(int eventId);
}