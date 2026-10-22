using ClubHub.DTOs;

namespace ClubHub.Services;

public interface INotificationService
{
    Task<NotificationResponse?> CreateAsync(CreateNotificationRequest request);
    Task<List<NotificationResponse>?> GetByUserAsync(int userId);
    Task<List<NotificationResponse>?> GetUnreadByUserAsync(int userId);
    Task<NotificationResponse?> MarkAsReadAsync(int notificationId);
    Task<bool> MarkAllAsReadAsync(int userId);
    Task<bool> DeleteAsync(int notificationId);
}