using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _context;

    public NotificationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<NotificationResponse?> CreateAsync(CreateNotificationRequest request)
    {
        var notification = new Notification
        {
            UserId = request.UserId,
            Title = request.Title.Trim(),
            Message = request.Message.Trim(),
            Type = request.Type.Trim(),
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return MapToResponse(notification);
    }

    public async Task<List<NotificationResponse>?> GetByUserAsync(int userId)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(notification => notification.UserId == userId)
            .OrderByDescending(notification => notification.CreatedAt)
            .Select(notification => new NotificationResponse
            {
                NotificationId = notification.NotificationId,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<NotificationResponse>?> GetUnreadByUserAsync(int userId)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(notification => notification.UserId == userId && !notification.IsRead)
            .OrderByDescending(notification => notification.CreatedAt)
            .Select(notification => new NotificationResponse
            {
                NotificationId = notification.NotificationId,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<NotificationResponse?> MarkAsReadAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);

        if (notification == null)
        {
            return null;
        }

        notification.IsRead = true;

        await _context.SaveChangesAsync();

        return MapToResponse(notification);
    }

    public async Task<bool> MarkAllAsReadAsync(int userId)
    {
        var unread = await _context.Notifications
            .Where(notification => notification.UserId == userId && !notification.IsRead)
            .ToListAsync();

        foreach (var notification in unread)
        {
            notification.IsRead = true;
        }

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);

        if (notification == null)
        {
            return false;
        }

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();

        return true;
    }

    private static NotificationResponse MapToResponse(Notification notification)
    {
        return new NotificationResponse
        {
            NotificationId = notification.NotificationId,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            Type = notification.Type,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }
}