using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class EventRsvpService : IEventRsvpService
{
    private readonly AppDbContext _context;

    public EventRsvpService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EventRsvpResponse?> CreateAsync(CreateEventRsvpRequest request)
    {
        var eventItem = await _context.Events.FindAsync(request.EventId);

        if (eventItem == null)
        {
            return null;
        }

        if (eventItem.Status != "Approved" || !eventItem.IsActive)
        {
            return null;
        }

        bool alreadyRsvpd = await _context.EventRsvps
            .AnyAsync(eventRsvp => eventRsvp.EventId == request.EventId && eventRsvp.UserId == request.UserId);

        if (alreadyRsvpd)
        {
            return null;
        }

        if (eventItem.Capacity != null)
        {
            int currentCount = await _context.EventRsvps
                .CountAsync(eventRsvp => eventRsvp.EventId == request.EventId);

            if (currentCount >= eventItem.Capacity)
            {
                return null;
            }
        }

        var eventRsvp = new EventRsvp
        {
            EventId = request.EventId,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.EventRsvps.Add(eventRsvp);
        await _context.SaveChangesAsync();

        return MapToResponse(eventRsvp);
    }

    public async Task<List<EventRsvpResponse>?> GetByEventAsync(int eventId)
    {
        return await _context.EventRsvps
            .AsNoTracking()
            .Where(eventRsvp => eventRsvp.EventId == eventId)
            .OrderBy(eventRsvp => eventRsvp.CreatedAt)
            .Select(eventRsvp => new EventRsvpResponse
            {
                EventRsvpId = eventRsvp.EventRsvpId,
                EventId = eventRsvp.EventId,
                UserId = eventRsvp.UserId,
                CreatedAt = eventRsvp.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<EventRsvpResponse>?> GetByUserAsync(int userId)
    {
        return await _context.EventRsvps
            .AsNoTracking()
            .Where(eventRsvp => eventRsvp.UserId == userId)
            .OrderByDescending(eventRsvp => eventRsvp.CreatedAt)
            .Select(eventRsvp => new EventRsvpResponse
            {
                EventRsvpId = eventRsvp.EventRsvpId,
                EventId = eventRsvp.EventId,
                UserId = eventRsvp.UserId,
                CreatedAt = eventRsvp.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int eventRsvpId)
    {
        var eventRsvp = await _context.EventRsvps.FindAsync(eventRsvpId);

        if (eventRsvp == null)
        {
            return false;
        }

        _context.EventRsvps.Remove(eventRsvp);
        await _context.SaveChangesAsync();

        return true;
    }

    private static EventRsvpResponse MapToResponse(EventRsvp eventRsvp)
    {
        return new EventRsvpResponse
        {
            EventRsvpId = eventRsvp.EventRsvpId,
            EventId = eventRsvp.EventId,
            UserId = eventRsvp.UserId,
            CreatedAt = eventRsvp.CreatedAt
        };
    }
}