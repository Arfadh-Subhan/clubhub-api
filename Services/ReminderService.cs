using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ReminderService : IReminderService
{
    private readonly AppDbContext _context;

    public ReminderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ReminderResponse?> CreateAsync(CreateReminderRequest request)
    {
        var eventItem = await _context.Events.FindAsync(request.EventId);

        if (eventItem == null)
        {
            return null;
        }

        bool alreadyExists = await _context.Reminders
            .AnyAsync(reminder => reminder.EventId == request.EventId && reminder.UserId == request.UserId);

        if (alreadyExists)
        {
            return null;
        }

        var reminder = new Reminder
        {
            EventId = request.EventId,
            UserId = request.UserId,
            RemindAt = eventItem.EventStartAt.AddHours(-24),
            ExpireAt = eventItem.EventStartAt.AddHours(24),
            Message = request.Message,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reminders.Add(reminder);
        await _context.SaveChangesAsync();

        return MapToResponse(reminder);
    }

    public async Task<List<ReminderResponse>?> GetByUserAsync(int userId)
    {
        return await _context.Reminders
            .AsNoTracking()
            .Where(reminder => reminder.UserId == userId)
            .OrderBy(reminder => reminder.RemindAt)
            .Select(reminder => new ReminderResponse
            {
                ReminderId = reminder.ReminderId,
                EventId = reminder.EventId,
                UserId = reminder.UserId,
                RemindAt = reminder.RemindAt,
                ExpireAt = reminder.ExpireAt,
                Message = reminder.Message,
                CreatedAt = reminder.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<ReminderResponse>?> GetActiveByUserAsync(int userId)
    {
        var now = DateTime.UtcNow;

        return await _context.Reminders
            .AsNoTracking()
            .Where(reminder => reminder.UserId == userId && now >= reminder.RemindAt && now <= reminder.ExpireAt)
            .OrderBy(reminder => reminder.RemindAt)
            .Select(reminder => new ReminderResponse
            {
                ReminderId = reminder.ReminderId,
                EventId = reminder.EventId,
                UserId = reminder.UserId,
                RemindAt = reminder.RemindAt,
                ExpireAt = reminder.ExpireAt,
                Message = reminder.Message,
                CreatedAt = reminder.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int reminderId)
    {
        var reminder = await _context.Reminders.FindAsync(reminderId);

        if (reminder == null)
        {
            return false;
        }

        _context.Reminders.Remove(reminder);
        await _context.SaveChangesAsync();

        return true;
    }

    private static ReminderResponse MapToResponse(Reminder reminder)
    {
        return new ReminderResponse
        {
            ReminderId = reminder.ReminderId,
            EventId = reminder.EventId,
            UserId = reminder.UserId,
            RemindAt = reminder.RemindAt,
            ExpireAt = reminder.ExpireAt,
            Message = reminder.Message,
            CreatedAt = reminder.CreatedAt
        };
    }
}