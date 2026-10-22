using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IReminderService
{
    Task<ReminderResponse?> CreateAsync(CreateReminderRequest request);
    Task<List<ReminderResponse>?> GetByUserAsync(int userId);
    Task<List<ReminderResponse>?> GetActiveByUserAsync(int userId);
    Task<bool> DeleteAsync(int reminderId);
}