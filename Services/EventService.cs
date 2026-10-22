using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _context;

    public EventService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EventResponse?> CreateAsync(CreateEventRequest request)
    {
        var eventItem = new Event
        {
            ClubId = request.ClubId,
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            EventDate = request.EventDate,
            EventTime = request.EventTime.Trim(),
            EventStartAt = BuildEventStartAt(request.EventDate, request.EventTime),
            Venue = request.Venue.Trim(),
            Poster = request.Poster,
            Links = request.Links,
            Capacity = request.Capacity,
            EstimatedBudget = request.EstimatedBudget,
            PermissionProof = request.PermissionProof,
            Status = "Pending",
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Events.Add(eventItem);
        await _context.SaveChangesAsync();

        return await MapToResponseAsync(eventItem);
    }

    public async Task<PageResult<EventResponse>?> GetUpcomingAsync(int pageNumber, int pageSize)
    {
        var now = DateTime.UtcNow;

        var query = _context.Events
            .AsNoTracking()
            .Where(eventItem => eventItem.Status == "Approved" && eventItem.IsActive && eventItem.EventStartAt >= now)
            .OrderBy(eventItem => eventItem.EventStartAt);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(eventItem => new EventResponse
            {
                EventId = eventItem.EventId,
                ClubId = eventItem.ClubId,
                Name = eventItem.Name,
                Description = eventItem.Description,
                EventDate = eventItem.EventDate,
                EventTime = eventItem.EventTime,
                EventStartAt = eventItem.EventStartAt,
                Venue = eventItem.Venue,
                Poster = eventItem.Poster,
                Links = eventItem.Links,
                Capacity = eventItem.Capacity,
                EstimatedBudget = eventItem.EstimatedBudget,
                PermissionProof = eventItem.PermissionProof,
                Status = eventItem.Status,
                ReviewedBy = eventItem.ReviewedBy,
                ReviewedAt = eventItem.ReviewedAt,
                RejectionReason = eventItem.RejectionReason,
                CreatedBy = eventItem.CreatedBy,
                CreatedAt = eventItem.CreatedAt,
                UpdatedAt = eventItem.UpdatedAt,
                IsActive = eventItem.IsActive,
                AttendingCount = _context.EventRsvps.Count(eventRsvp => eventRsvp.EventId == eventItem.EventId)
            })
            .ToListAsync();

        return new PageResult<EventResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<List<EventResponse>?> GetByClubAsync(int clubId)
    {
        return await _context.Events
            .AsNoTracking()
            .Where(eventItem => eventItem.ClubId == clubId && eventItem.Status == "Approved" && eventItem.IsActive)
            .OrderByDescending(eventItem => eventItem.EventStartAt)
            .Select(eventItem => new EventResponse
            {
                EventId = eventItem.EventId,
                ClubId = eventItem.ClubId,
                Name = eventItem.Name,
                Description = eventItem.Description,
                EventDate = eventItem.EventDate,
                EventTime = eventItem.EventTime,
                EventStartAt = eventItem.EventStartAt,
                Venue = eventItem.Venue,
                Poster = eventItem.Poster,
                Links = eventItem.Links,
                Capacity = eventItem.Capacity,
                EstimatedBudget = eventItem.EstimatedBudget,
                PermissionProof = eventItem.PermissionProof,
                Status = eventItem.Status,
                ReviewedBy = eventItem.ReviewedBy,
                ReviewedAt = eventItem.ReviewedAt,
                RejectionReason = eventItem.RejectionReason,
                CreatedBy = eventItem.CreatedBy,
                CreatedAt = eventItem.CreatedAt,
                UpdatedAt = eventItem.UpdatedAt,
                IsActive = eventItem.IsActive,
                AttendingCount = _context.EventRsvps.Count(eventRsvp => eventRsvp.EventId == eventItem.EventId)
            })
            .ToListAsync();
    }

    public async Task<EventResponse?> GetByIdAsync(int eventId)
    {
        return await _context.Events
            .AsNoTracking()
            .Where(eventItem => eventItem.EventId == eventId)
            .Select(eventItem => new EventResponse
            {
                EventId = eventItem.EventId,
                ClubId = eventItem.ClubId,
                Name = eventItem.Name,
                Description = eventItem.Description,
                EventDate = eventItem.EventDate,
                EventTime = eventItem.EventTime,
                EventStartAt = eventItem.EventStartAt,
                Venue = eventItem.Venue,
                Poster = eventItem.Poster,
                Links = eventItem.Links,
                Capacity = eventItem.Capacity,
                EstimatedBudget = eventItem.EstimatedBudget,
                PermissionProof = eventItem.PermissionProof,
                Status = eventItem.Status,
                ReviewedBy = eventItem.ReviewedBy,
                ReviewedAt = eventItem.ReviewedAt,
                RejectionReason = eventItem.RejectionReason,
                CreatedBy = eventItem.CreatedBy,
                CreatedAt = eventItem.CreatedAt,
                UpdatedAt = eventItem.UpdatedAt,
                IsActive = eventItem.IsActive,
                AttendingCount = _context.EventRsvps.Count(eventRsvp => eventRsvp.EventId == eventItem.EventId)
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<EventResponse>?> GetPendingAsync()
    {
        return await _context.Events
            .AsNoTracking()
            .Where(eventItem => eventItem.Status == "Pending" && eventItem.IsActive)
            .OrderBy(eventItem => eventItem.CreatedAt)
            .Select(eventItem => new EventResponse
            {
                EventId = eventItem.EventId,
                ClubId = eventItem.ClubId,
                Name = eventItem.Name,
                Description = eventItem.Description,
                EventDate = eventItem.EventDate,
                EventTime = eventItem.EventTime,
                EventStartAt = eventItem.EventStartAt,
                Venue = eventItem.Venue,
                Poster = eventItem.Poster,
                Links = eventItem.Links,
                Capacity = eventItem.Capacity,
                EstimatedBudget = eventItem.EstimatedBudget,
                PermissionProof = eventItem.PermissionProof,
                Status = eventItem.Status,
                ReviewedBy = eventItem.ReviewedBy,
                ReviewedAt = eventItem.ReviewedAt,
                RejectionReason = eventItem.RejectionReason,
                CreatedBy = eventItem.CreatedBy,
                CreatedAt = eventItem.CreatedAt,
                UpdatedAt = eventItem.UpdatedAt,
                IsActive = eventItem.IsActive,
                AttendingCount = _context.EventRsvps.Count(eventRsvp => eventRsvp.EventId == eventItem.EventId)
            })
            .ToListAsync();
    }

    public async Task<EventResponse?> UpdateAsync(int eventId, UpdateEventRequest request)
    {
        var eventItem = await _context.Events.FindAsync(eventId);

        if (eventItem == null)
        {
            return null;
        }

        eventItem.Name = request.Name.Trim();
        eventItem.Description = request.Description.Trim();
        eventItem.EventDate = request.EventDate;
        eventItem.EventTime = request.EventTime.Trim();
        eventItem.EventStartAt = BuildEventStartAt(request.EventDate, request.EventTime);
        eventItem.Venue = request.Venue.Trim();
        eventItem.Poster = request.Poster;
        eventItem.Links = request.Links;
        eventItem.Capacity = request.Capacity;
        eventItem.EstimatedBudget = request.EstimatedBudget;
        eventItem.PermissionProof = request.PermissionProof;
        eventItem.Status = "Pending";
        eventItem.ReviewedBy = null;
        eventItem.ReviewedAt = null;
        eventItem.RejectionReason = null;
        eventItem.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await MapToResponseAsync(eventItem);
    }

    public async Task<EventResponse?> ApproveAsync(int eventId, int reviewedBy)
    {
        var eventItem = await _context.Events.FindAsync(eventId);

        if (eventItem == null)
        {
            return null;
        }

        eventItem.Status = "Approved";
        eventItem.ReviewedBy = reviewedBy;
        eventItem.ReviewedAt = DateTime.UtcNow;
        eventItem.RejectionReason = null;

        var recipients = new List<int>();

        if (eventItem.ClubId != null)
        {
            var clubMemberIds = await _context.ClubMembers
                .AsNoTracking()
                .Where(clubMember => clubMember.ClubId == eventItem.ClubId
                    && clubMember.Status == "Approved"
                    && clubMember.UserId != eventItem.CreatedBy)
                .Select(clubMember => clubMember.UserId)
                .ToListAsync();

            recipients.AddRange(clubMemberIds);
        }
        else
        {
            var allStudentIds = await _context.Users
                .AsNoTracking()
                .Where(user => user.Role == "Student" && user.IsActive && user.UserId != eventItem.CreatedBy)
                .Select(user => user.UserId)
                .ToListAsync();

            recipients.AddRange(allStudentIds);
        }

        var notificationTime = DateTime.UtcNow;

        foreach (var recipientId in recipients.Distinct())
        {
            _context.Notifications.Add(new Notification
            {
                UserId = recipientId,
                Title = "New event: " + eventItem.Name,
                Message = "A new event on " + eventItem.EventStartAt.ToString("dd MMM yyyy") + " at " + eventItem.Venue + " is now live.",
                Type = "Event",
                IsRead = false,
                CreatedAt = notificationTime
            });
        }

        await _context.SaveChangesAsync();

        return await MapToResponseAsync(eventItem);
    }

    public async Task<EventResponse?> RejectAsync(int eventId, int reviewedBy, string reason)
    {
        var eventItem = await _context.Events.FindAsync(eventId);

        if (eventItem == null)
        {
            return null;
        }

        eventItem.Status = "Rejected";
        eventItem.ReviewedBy = reviewedBy;
        eventItem.ReviewedAt = DateTime.UtcNow;
        eventItem.RejectionReason = reason;

        _context.Notifications.Add(new Notification
        {
            UserId = eventItem.CreatedBy,
            Title = "Event rejected: " + eventItem.Name,
            Message = "Your event was rejected. Reason: " + reason,
            Type = "Event",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return await MapToResponseAsync(eventItem);
    }

    public async Task<bool> DeleteAsync(int eventId)
    {
        var eventItem = await _context.Events.FindAsync(eventId);

        if (eventItem == null)
        {
            return false;
        }

        var rsvps = await _context.EventRsvps
            .Where(r => r.EventId == eventId)
            .ToListAsync();
        _context.EventRsvps.RemoveRange(rsvps);

        var likes = await _context.Likes
            .Where(l => l.EventId == eventId)
            .ToListAsync();
        _context.Likes.RemoveRange(likes);

        var reminders = await _context.Reminders
            .Where(r => r.EventId == eventId)
            .ToListAsync();
        _context.Reminders.RemoveRange(reminders);

        _context.Events.Remove(eventItem);

        await _context.SaveChangesAsync();

        return true;
    }

    private static DateTime BuildEventStartAt(DateTime eventDate, string eventTime)
    {
        DateTime parsedTime;

        bool parsed = DateTime.TryParse(eventTime, out parsedTime);

        if (!parsed)
        {
            parsedTime = eventDate.Date;
        }

        return new DateTime(
            eventDate.Year,
            eventDate.Month,
            eventDate.Day,
            parsedTime.Hour,
            parsedTime.Minute,
            0);
    }

    private async Task<EventResponse> MapToResponseAsync(Event eventItem)
    {
        var attendingCount = await _context.EventRsvps
            .CountAsync(eventRsvp => eventRsvp.EventId == eventItem.EventId);

        return new EventResponse
        {
            EventId = eventItem.EventId,
            ClubId = eventItem.ClubId,
            Name = eventItem.Name,
            Description = eventItem.Description,
            EventDate = eventItem.EventDate,
            EventTime = eventItem.EventTime,
            EventStartAt = eventItem.EventStartAt,
            Venue = eventItem.Venue,
            Poster = eventItem.Poster,
            Links = eventItem.Links,
            Capacity = eventItem.Capacity,
            EstimatedBudget = eventItem.EstimatedBudget,
            PermissionProof = eventItem.PermissionProof,
            Status = eventItem.Status,
            ReviewedBy = eventItem.ReviewedBy,
            ReviewedAt = eventItem.ReviewedAt,
            RejectionReason = eventItem.RejectionReason,
            CreatedBy = eventItem.CreatedBy,
            CreatedAt = eventItem.CreatedAt,
            UpdatedAt = eventItem.UpdatedAt,
            IsActive = eventItem.IsActive,
            AttendingCount = attendingCount
        };
    }
}