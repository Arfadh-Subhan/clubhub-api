using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IEventRsvpService
{
    Task<EventRsvpResponse?> CreateAsync(CreateEventRsvpRequest request);
    Task<List<EventRsvpResponse>?> GetByEventAsync(int eventId);
    Task<List<EventRsvpResponse>?> GetByUserAsync(int userId);
    Task<bool> DeleteAsync(int eventRsvpId);
}