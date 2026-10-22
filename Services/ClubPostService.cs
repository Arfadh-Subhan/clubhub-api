using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ClubPostService : IClubPostService
{
    private readonly AppDbContext _context;

    public ClubPostService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClubPostResponse?> CreateAsync(CreateClubPostRequest request)
    {
        var clubPost = new ClubPost
        {
            ClubId = request.ClubId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Category = request.Category.Trim(),
            CoverImage = request.CoverImage,
            Links = request.Links,
            Status = "Pending",
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.ClubPosts.Add(clubPost);
        await _context.SaveChangesAsync();

        return MapToResponse(clubPost);
    }

    public async Task<PageResult<ClubPostResponse>?> GetAllApprovedAsync(int pageNumber, int pageSize)
    {
        var query = _context.ClubPosts
            .AsNoTracking()
            .Where(clubPost => clubPost.Status == "Approved" && clubPost.IsActive)
            .OrderByDescending(clubPost => clubPost.CreatedAt);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(clubPost => new ClubPostResponse
            {
                ClubPostId = clubPost.ClubPostId,
                ClubId = clubPost.ClubId,
                Title = clubPost.Title,
                Description = clubPost.Description,
                Category = clubPost.Category,
                CoverImage = clubPost.CoverImage,
                Links = clubPost.Links,
                Status = clubPost.Status,
                ReviewedBy = clubPost.ReviewedBy,
                ReviewedAt = clubPost.ReviewedAt,
                RejectionReason = clubPost.RejectionReason,
                CreatedBy = clubPost.CreatedBy,
                CreatedAt = clubPost.CreatedAt,
                UpdatedAt = clubPost.UpdatedAt,
                IsActive = clubPost.IsActive
            })
            .ToListAsync();

        return new PageResult<ClubPostResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ClubPostResponse?> GetByIdAsync(int clubPostId)
    {
        return await _context.ClubPosts
            .AsNoTracking()
            .Where(clubPost => clubPost.ClubPostId == clubPostId)
            .Select(clubPost => new ClubPostResponse
            {
                ClubPostId = clubPost.ClubPostId,
                ClubId = clubPost.ClubId,
                Title = clubPost.Title,
                Description = clubPost.Description,
                Category = clubPost.Category,
                CoverImage = clubPost.CoverImage,
                Links = clubPost.Links,
                Status = clubPost.Status,
                ReviewedBy = clubPost.ReviewedBy,
                ReviewedAt = clubPost.ReviewedAt,
                RejectionReason = clubPost.RejectionReason,
                CreatedBy = clubPost.CreatedBy,
                CreatedAt = clubPost.CreatedAt,
                UpdatedAt = clubPost.UpdatedAt,
                IsActive = clubPost.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<ClubPostResponse>?> GetByClubAsync(int clubId)
    {
        return await _context.ClubPosts
            .AsNoTracking()
            .Where(clubPost => clubPost.ClubId == clubId && clubPost.Status == "Approved" && clubPost.IsActive)
            .OrderByDescending(clubPost => clubPost.CreatedAt)
            .Select(clubPost => new ClubPostResponse
            {
                ClubPostId = clubPost.ClubPostId,
                ClubId = clubPost.ClubId,
                Title = clubPost.Title,
                Description = clubPost.Description,
                Category = clubPost.Category,
                CoverImage = clubPost.CoverImage,
                Links = clubPost.Links,
                Status = clubPost.Status,
                ReviewedBy = clubPost.ReviewedBy,
                ReviewedAt = clubPost.ReviewedAt,
                RejectionReason = clubPost.RejectionReason,
                CreatedBy = clubPost.CreatedBy,
                CreatedAt = clubPost.CreatedAt,
                UpdatedAt = clubPost.UpdatedAt,
                IsActive = clubPost.IsActive
            })
            .ToListAsync();
    }

    public async Task<List<ClubPostResponse>?> GetPendingAsync()
    {
        return await _context.ClubPosts
            .AsNoTracking()
            .Where(clubPost => clubPost.Status == "Pending" && clubPost.IsActive)
            .OrderBy(clubPost => clubPost.CreatedAt)
            .Select(clubPost => new ClubPostResponse
            {
                ClubPostId = clubPost.ClubPostId,
                ClubId = clubPost.ClubId,
                Title = clubPost.Title,
                Description = clubPost.Description,
                Category = clubPost.Category,
                CoverImage = clubPost.CoverImage,
                Links = clubPost.Links,
                Status = clubPost.Status,
                ReviewedBy = clubPost.ReviewedBy,
                ReviewedAt = clubPost.ReviewedAt,
                RejectionReason = clubPost.RejectionReason,
                CreatedBy = clubPost.CreatedBy,
                CreatedAt = clubPost.CreatedAt,
                UpdatedAt = clubPost.UpdatedAt,
                IsActive = clubPost.IsActive
            })
            .ToListAsync();
    }

    public async Task<ClubPostResponse?> UpdateAsync(int clubPostId, UpdateClubPostRequest request)
    {
        var clubPost = await _context.ClubPosts.FindAsync(clubPostId);

        if (clubPost == null)
        {
            return null;
        }

        clubPost.Title = request.Title.Trim();
        clubPost.Description = request.Description.Trim();
        clubPost.Category = request.Category.Trim();
        clubPost.CoverImage = request.CoverImage;
        clubPost.Links = request.Links;
        clubPost.Status = "Pending";
        clubPost.ReviewedBy = null;
        clubPost.ReviewedAt = null;
        clubPost.RejectionReason = null;
        clubPost.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(clubPost);
    }

    public async Task<ClubPostResponse?> ApproveAsync(int clubPostId, int reviewedBy)
    {
        var clubPost = await _context.ClubPosts.FindAsync(clubPostId);

        if (clubPost == null)
        {
            return null;
        }

        clubPost.Status = "Approved";
        clubPost.ReviewedBy = reviewedBy;
        clubPost.ReviewedAt = DateTime.UtcNow;
        clubPost.RejectionReason = null;

        await _context.SaveChangesAsync();

        return MapToResponse(clubPost);
    }

    public async Task<ClubPostResponse?> RejectAsync(int clubPostId, int reviewedBy, string reason)
    {
        var clubPost = await _context.ClubPosts.FindAsync(clubPostId);

        if (clubPost == null)
        {
            return null;
        }

        clubPost.Status = "Rejected";
        clubPost.ReviewedBy = reviewedBy;
        clubPost.ReviewedAt = DateTime.UtcNow;
        clubPost.RejectionReason = reason;

        await _context.SaveChangesAsync();

        return MapToResponse(clubPost);
    }

    public async Task<bool> DeleteAsync(int clubPostId)
    {
        var clubPost = await _context.ClubPosts.FindAsync(clubPostId);

        if (clubPost == null)
        {
            return false;
        }

        clubPost.IsActive = false;
        clubPost.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private static ClubPostResponse MapToResponse(ClubPost clubPost)
    {
        return new ClubPostResponse
        {
            ClubPostId = clubPost.ClubPostId,
            ClubId = clubPost.ClubId,
            Title = clubPost.Title,
            Description = clubPost.Description,
            Category = clubPost.Category,
            CoverImage = clubPost.CoverImage,
            Links = clubPost.Links,
            Status = clubPost.Status,
            ReviewedBy = clubPost.ReviewedBy,
            ReviewedAt = clubPost.ReviewedAt,
            RejectionReason = clubPost.RejectionReason,
            CreatedBy = clubPost.CreatedBy,
            CreatedAt = clubPost.CreatedAt,
            UpdatedAt = clubPost.UpdatedAt,
            IsActive = clubPost.IsActive
        };
    }
}