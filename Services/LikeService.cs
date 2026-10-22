using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class LikeService : ILikeService
{
    private readonly AppDbContext _context;

    public LikeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LikeResponse?> ToggleAsync(ToggleLikeRequest request)
    {
        var eventItem = await _context.Events.FindAsync(request.EventId);

        if (eventItem == null)
        {
            return null;
        }

        var existingLike = await _context.Likes
            .FirstOrDefaultAsync(like => like.EventId == request.EventId && like.UserId == request.UserId);

        if (existingLike != null)
        {
            _context.Likes.Remove(existingLike);
            await _context.SaveChangesAsync();
        }
        else
        {
            var like = new Like
            {
                EventId = request.EventId,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
        }

        int likeCount = await _context.Likes
            .CountAsync(like => like.EventId == request.EventId);

        return new LikeResponse
        {
            EventId = request.EventId,
            LikeCount = likeCount
        };
    }

    public async Task<LikeResponse?> GetByEventAsync(int eventId)
    {
        var eventItem = await _context.Events.FindAsync(eventId);

        if (eventItem == null)
        {
            return null;
        }

        int likeCount = await _context.Likes
            .CountAsync(like => like.EventId == eventId);

        return new LikeResponse
        {
            EventId = eventId,
            LikeCount = likeCount
        };
    }
}