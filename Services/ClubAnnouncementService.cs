using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ClubAnnouncementService : IClubAnnouncementService
{
    private readonly AppDbContext _context;

    public ClubAnnouncementService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClubAnnouncementResponse?> CreateAsync(CreateClubAnnouncementRequest request)
    {
        var clubAnnouncement = new ClubAnnouncement
        {
            ClubId = request.ClubId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Links = request.Links,
            Status = "Approved",
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.ClubAnnouncements.Add(clubAnnouncement);
        await _context.SaveChangesAsync();

        return MapToResponse(clubAnnouncement);
    }

    public async Task<PageResult<ClubAnnouncementResponse>?> GetAllApprovedAsync(int pageNumber, int pageSize)
    {
        var query = _context.ClubAnnouncements
            .AsNoTracking()
            .Where(clubAnnouncement => clubAnnouncement.Status == "Approved" && clubAnnouncement.IsActive)
            .OrderByDescending(clubAnnouncement => clubAnnouncement.CreatedAt);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(clubAnnouncement => new ClubAnnouncementResponse
            {
                ClubAnnouncementId = clubAnnouncement.ClubAnnouncementId,
                ClubId = clubAnnouncement.ClubId,
                Title = clubAnnouncement.Title,
                Description = clubAnnouncement.Description,
                Links = clubAnnouncement.Links,
                Status = clubAnnouncement.Status,
                ReviewedBy = clubAnnouncement.ReviewedBy,
                ReviewedAt = clubAnnouncement.ReviewedAt,
                RejectionReason = clubAnnouncement.RejectionReason,
                CreatedBy = clubAnnouncement.CreatedBy,
                CreatedAt = clubAnnouncement.CreatedAt,
                UpdatedAt = clubAnnouncement.UpdatedAt,
                IsActive = clubAnnouncement.IsActive
            })
            .ToListAsync();

        return new PageResult<ClubAnnouncementResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ClubAnnouncementResponse?> GetByIdAsync(int clubAnnouncementId)
    {
        return await _context.ClubAnnouncements
            .AsNoTracking()
            .Where(clubAnnouncement => clubAnnouncement.ClubAnnouncementId == clubAnnouncementId)
            .Select(clubAnnouncement => new ClubAnnouncementResponse
            {
                ClubAnnouncementId = clubAnnouncement.ClubAnnouncementId,
                ClubId = clubAnnouncement.ClubId,
                Title = clubAnnouncement.Title,
                Description = clubAnnouncement.Description,
                Links = clubAnnouncement.Links,
                Status = clubAnnouncement.Status,
                ReviewedBy = clubAnnouncement.ReviewedBy,
                ReviewedAt = clubAnnouncement.ReviewedAt,
                RejectionReason = clubAnnouncement.RejectionReason,
                CreatedBy = clubAnnouncement.CreatedBy,
                CreatedAt = clubAnnouncement.CreatedAt,
                UpdatedAt = clubAnnouncement.UpdatedAt,
                IsActive = clubAnnouncement.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<ClubAnnouncementResponse>?> GetByClubAsync(int clubId)
    {
        return await _context.ClubAnnouncements
            .AsNoTracking()
            .Where(clubAnnouncement => clubAnnouncement.ClubId == clubId && clubAnnouncement.Status == "Approved" && clubAnnouncement.IsActive)
            .OrderByDescending(clubAnnouncement => clubAnnouncement.CreatedAt)
            .Select(clubAnnouncement => new ClubAnnouncementResponse
            {
                ClubAnnouncementId = clubAnnouncement.ClubAnnouncementId,
                ClubId = clubAnnouncement.ClubId,
                Title = clubAnnouncement.Title,
                Description = clubAnnouncement.Description,
                Links = clubAnnouncement.Links,
                Status = clubAnnouncement.Status,
                ReviewedBy = clubAnnouncement.ReviewedBy,
                ReviewedAt = clubAnnouncement.ReviewedAt,
                RejectionReason = clubAnnouncement.RejectionReason,
                CreatedBy = clubAnnouncement.CreatedBy,
                CreatedAt = clubAnnouncement.CreatedAt,
                UpdatedAt = clubAnnouncement.UpdatedAt,
                IsActive = clubAnnouncement.IsActive
            })
            .ToListAsync();
    }

    public async Task<List<ClubAnnouncementResponse>?> GetPendingAsync()
    {
        return await _context.ClubAnnouncements
            .AsNoTracking()
            .Where(clubAnnouncement => clubAnnouncement.Status == "Pending" && clubAnnouncement.IsActive)
            .OrderBy(clubAnnouncement => clubAnnouncement.CreatedAt)
            .Select(clubAnnouncement => new ClubAnnouncementResponse
            {
                ClubAnnouncementId = clubAnnouncement.ClubAnnouncementId,
                ClubId = clubAnnouncement.ClubId,
                Title = clubAnnouncement.Title,
                Description = clubAnnouncement.Description,
                Links = clubAnnouncement.Links,
                Status = clubAnnouncement.Status,
                ReviewedBy = clubAnnouncement.ReviewedBy,
                ReviewedAt = clubAnnouncement.ReviewedAt,
                RejectionReason = clubAnnouncement.RejectionReason,
                CreatedBy = clubAnnouncement.CreatedBy,
                CreatedAt = clubAnnouncement.CreatedAt,
                UpdatedAt = clubAnnouncement.UpdatedAt,
                IsActive = clubAnnouncement.IsActive
            })
            .ToListAsync();
    }

    public async Task<ClubAnnouncementResponse?> UpdateAsync(int clubAnnouncementId, UpdateClubAnnouncementRequest request)
    {
        var clubAnnouncement = await _context.ClubAnnouncements.FindAsync(clubAnnouncementId);

        if (clubAnnouncement == null)
        {
            return null;
        }

        clubAnnouncement.Title = request.Title.Trim();
        clubAnnouncement.Description = request.Description.Trim();
        clubAnnouncement.Links = request.Links;
        clubAnnouncement.Status = "Approved";
        clubAnnouncement.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(clubAnnouncement);
    }

    public async Task<ClubAnnouncementResponse?> ApproveAsync(int clubAnnouncementId, int reviewedBy)
    {
        var clubAnnouncement = await _context.ClubAnnouncements.FindAsync(clubAnnouncementId);

        if (clubAnnouncement == null)
        {
            return null;
        }

        clubAnnouncement.Status = "Approved";
        clubAnnouncement.ReviewedBy = reviewedBy;
        clubAnnouncement.ReviewedAt = DateTime.UtcNow;
        clubAnnouncement.RejectionReason = null;

        await _context.SaveChangesAsync();

        return MapToResponse(clubAnnouncement);
    }

    public async Task<ClubAnnouncementResponse?> RejectAsync(int clubAnnouncementId, int reviewedBy, string reason)
    {
        var clubAnnouncement = await _context.ClubAnnouncements.FindAsync(clubAnnouncementId);

        if (clubAnnouncement == null)
        {
            return null;
        }

        clubAnnouncement.Status = "Rejected";
        clubAnnouncement.ReviewedBy = reviewedBy;
        clubAnnouncement.ReviewedAt = DateTime.UtcNow;
        clubAnnouncement.RejectionReason = reason;

        await _context.SaveChangesAsync();

        return MapToResponse(clubAnnouncement);
    }

    public async Task<bool> DeleteAsync(int clubAnnouncementId)
    {
        var clubAnnouncement = await _context.ClubAnnouncements.FindAsync(clubAnnouncementId);

        if (clubAnnouncement == null)
        {
            return false;
        }

        clubAnnouncement.IsActive = false;
        clubAnnouncement.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private static ClubAnnouncementResponse MapToResponse(ClubAnnouncement clubAnnouncement)
    {
        return new ClubAnnouncementResponse
        {
            ClubAnnouncementId = clubAnnouncement.ClubAnnouncementId,
            ClubId = clubAnnouncement.ClubId,
            Title = clubAnnouncement.Title,
            Description = clubAnnouncement.Description,
            Links = clubAnnouncement.Links,
            Status = clubAnnouncement.Status,
            ReviewedBy = clubAnnouncement.ReviewedBy,
            ReviewedAt = clubAnnouncement.ReviewedAt,
            RejectionReason = clubAnnouncement.RejectionReason,
            CreatedBy = clubAnnouncement.CreatedBy,
            CreatedAt = clubAnnouncement.CreatedAt,
            UpdatedAt = clubAnnouncement.UpdatedAt,
            IsActive = clubAnnouncement.IsActive
        };
    }
}