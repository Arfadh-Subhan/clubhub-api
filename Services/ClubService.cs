using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ClubService : IClubService
{
    private readonly AppDbContext _context;

    public ClubService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClubResponse?> CreateAsync(CreateClubRequest request)
    {
        var normalizedName = request.Name.Trim();

        bool nameTaken = await _context.Clubs.AnyAsync(c => c.Name == normalizedName && c.IsActive);

        if (nameTaken)
        {
            return null;
        }

        var club = new Club
        {
            Name = normalizedName,
            Description = request.Description.Trim(),
            Category = request.Category.Trim(),
            Logo = request.Logo,
            CoverPhoto = request.CoverPhoto,
            Status = "Pending",
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Clubs.Add(club);
        await _context.SaveChangesAsync();

        return new ClubResponse
        {
            ClubId = club.ClubId,
            Name = club.Name,
            Description = club.Description,
            Category = club.Category,
            Logo = club.Logo,
            CoverPhoto = club.CoverPhoto,
            Status = club.Status,
            ReviewedBy = club.ReviewedBy,
            ReviewedAt = club.ReviewedAt,
            RejectionReason = club.RejectionReason,
            CreatedBy = club.CreatedBy,
            CreatedAt = club.CreatedAt,
            IsActive = club.IsActive
        };
    }

    public async Task<PageResult<ClubResponse>?> GetAllAsync(int pageNumber, int pageSize)
    {
        var query = _context.Clubs
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ClubResponse
            {
                ClubId = c.ClubId,
                Name = c.Name,
                Description = c.Description,
                Category = c.Category,
                Logo = c.Logo,
                CoverPhoto = c.CoverPhoto,
                Status = c.Status,
                ReviewedBy = c.ReviewedBy,
                ReviewedAt = c.ReviewedAt,
                RejectionReason = c.RejectionReason,
                CreatedBy = c.CreatedBy,
                CreatedAt = c.CreatedAt,
                IsActive = c.IsActive
            })
            .ToListAsync();

        return new PageResult<ClubResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ClubResponse?> GetByIdAsync(int id)
    {
        return await _context.Clubs
            .AsNoTracking()
            .Where(c => c.ClubId == id)
            .Select(c => new ClubResponse
            {
                ClubId = c.ClubId,
                Name = c.Name,
                Description = c.Description,
                Category = c.Category,
                Logo = c.Logo,
                CoverPhoto = c.CoverPhoto,
                Status = c.Status,
                ReviewedBy = c.ReviewedBy,
                ReviewedAt = c.ReviewedAt,
                RejectionReason = c.RejectionReason,
                CreatedBy = c.CreatedBy,
                CreatedAt = c.CreatedAt,
                IsActive = c.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ClubResponse?> UpdateAsync(int id, UpdateClubRequest request)
    {
        var club = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubId == id);

        if (club == null)
        {
            return null;
        }

        var normalizedName = request.Name.Trim();

        bool nameTaken = await _context.Clubs.AnyAsync(c => c.Name == normalizedName && c.ClubId != id && c.IsActive);

        if (nameTaken)
        {
            return null;
        }

        club.Name = normalizedName;
        club.Description = request.Description.Trim();
        club.Category = request.Category.Trim();
        club.Logo = request.Logo;
        club.CoverPhoto = request.CoverPhoto;
        club.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new ClubResponse
        {
            ClubId = club.ClubId,
            Name = club.Name,
            Description = club.Description,
            Category = club.Category,
            Logo = club.Logo,
            CoverPhoto = club.CoverPhoto,
            Status = club.Status,
            ReviewedBy = club.ReviewedBy,
            ReviewedAt = club.ReviewedAt,
            RejectionReason = club.RejectionReason,
            CreatedBy = club.CreatedBy,
            CreatedAt = club.CreatedAt,
            IsActive = club.IsActive
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var club = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubId == id);

        if (club == null)
        {
            return false;
        }

        var productIds = await _context.ClubProducts
            .Where(p => p.ClubId == id)
            .Select(p => p.ClubProductId)
            .ToListAsync();

        if (productIds.Count > 0)
        {
            var orders = await _context.Orders
                .Where(o => productIds.Contains(o.ClubProductId))
                .ToListAsync();
            _context.Orders.RemoveRange(orders);
        }

        var likes = await _context.Likes
            .Where(l => _context.Events.Any(e => e.EventId == l.EventId && e.ClubId == id))
            .ToListAsync();
        _context.Likes.RemoveRange(likes);

        var rsvps = await _context.EventRsvps
            .Where(r => _context.Events.Any(e => e.EventId == r.EventId && e.ClubId == id))
            .ToListAsync();
        _context.EventRsvps.RemoveRange(rsvps);

        var reminders = await _context.Reminders
            .Where(r => _context.Events.Any(e => e.EventId == r.EventId && e.ClubId == id))
            .ToListAsync();
        _context.Reminders.RemoveRange(reminders);

        var events = await _context.Events
            .Where(e => e.ClubId == id)
            .ToListAsync();
        _context.Events.RemoveRange(events);

        var galleryIds = await _context.ClubGalleries
            .Where(g => g.ClubId == id)
            .Select(g => g.ClubGalleryId)
            .ToListAsync();

        if (galleryIds.Count > 0)
        {
            var photos = await _context.GalleryPhotos
                .Where(p => galleryIds.Contains(p.ClubGalleryId))
                .ToListAsync();
            _context.GalleryPhotos.RemoveRange(photos);
        }

        var galleries = await _context.ClubGalleries
            .Where(g => g.ClubId == id)
            .ToListAsync();
        _context.ClubGalleries.RemoveRange(galleries);

        var meetings = await _context.ClubMeetings
            .Where(m => m.ClubId == id)
            .ToListAsync();
        _context.ClubMeetings.RemoveRange(meetings);

        var announcements = await _context.ClubAnnouncements
            .Where(a => a.ClubId == id)
            .ToListAsync();
        _context.ClubAnnouncements.RemoveRange(announcements);

        var posts = await _context.ClubPosts
            .Where(p => p.ClubId == id)
            .ToListAsync();
        _context.ClubPosts.RemoveRange(posts);

        var products = await _context.ClubProducts
            .Where(p => p.ClubId == id)
            .ToListAsync();
        _context.ClubProducts.RemoveRange(products);

        var chatMessages = await _context.ChatMessages
            .Where(c => c.ClubId == id)
            .ToListAsync();
        _context.ChatMessages.RemoveRange(chatMessages);

        var members = await _context.ClubMembers
            .Where(m => m.ClubId == id)
            .ToListAsync();
        _context.ClubMembers.RemoveRange(members);

        _context.Clubs.Remove(club);

        await _context.SaveChangesAsync();

        return true;
    }
}