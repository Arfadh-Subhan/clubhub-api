using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ClubRequestService : IClubRequestService
{
    private readonly AppDbContext _context;

    public ClubRequestService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClubRequestResponse?> SubmitAsync(SubmitClubRequest request)
    {
        var clubRequest = new ClubRequest
        {
            RequestedBy = request.RequestedBy,
            ProposedName = request.ProposedName.Trim(),
            Category = request.Category.Trim(),
            Description = request.Description.Trim(),
            Purpose = request.Purpose.Trim(),
            ExpectedMembers = request.ExpectedMembers,
            CoverLetter = request.CoverLetter,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _context.ClubRequests.Add(clubRequest);
        await _context.SaveChangesAsync();

        await NotifyIcbtAdminsAsync(
            "New club request",
            "A student proposed a new club: \"" + clubRequest.ProposedName + "\".");

        return MapToResponse(clubRequest);
    }

    public async Task<List<ClubRequestResponse>?> GetAllAsync()
    {
        return await _context.ClubRequests
            .AsNoTracking()
            .OrderByDescending(clubRequest => clubRequest.CreatedAt)
            .Select(clubRequest => new ClubRequestResponse
            {
                ClubRequestId = clubRequest.ClubRequestId,
                RequestedBy = clubRequest.RequestedBy,
                ProposedName = clubRequest.ProposedName,
                Category = clubRequest.Category,
                Description = clubRequest.Description,
                Purpose = clubRequest.Purpose,
                ExpectedMembers = clubRequest.ExpectedMembers,
                CoverLetter = clubRequest.CoverLetter,
                Status = clubRequest.Status,
                ReviewedBy = clubRequest.ReviewedBy,
                ReviewedAt = clubRequest.ReviewedAt,
                RejectionReason = clubRequest.RejectionReason,
                CreatedAt = clubRequest.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<ClubRequestResponse>?> GetPendingAsync()
    {
        return await _context.ClubRequests
            .AsNoTracking()
            .Where(clubRequest => clubRequest.Status == "Pending")
            .OrderBy(clubRequest => clubRequest.CreatedAt)
            .Select(clubRequest => new ClubRequestResponse
            {
                ClubRequestId = clubRequest.ClubRequestId,
                RequestedBy = clubRequest.RequestedBy,
                ProposedName = clubRequest.ProposedName,
                Category = clubRequest.Category,
                Description = clubRequest.Description,
                Purpose = clubRequest.Purpose,
                ExpectedMembers = clubRequest.ExpectedMembers,
                CoverLetter = clubRequest.CoverLetter,
                Status = clubRequest.Status,
                ReviewedBy = clubRequest.ReviewedBy,
                ReviewedAt = clubRequest.ReviewedAt,
                RejectionReason = clubRequest.RejectionReason,
                CreatedAt = clubRequest.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ClubRequestResponse?> ApproveAsync(int clubRequestId, int reviewedBy)
    {
        var clubRequest = await _context.ClubRequests.FindAsync(clubRequestId);

        if (clubRequest == null)
        {
            return null;
        }

        bool clubExists = await _context.Clubs.AnyAsync(club => club.Name == clubRequest.ProposedName);

        if (clubExists)
        {
            return null;
        }

        var club = new Club
        {
            Name = clubRequest.ProposedName,
            Description = clubRequest.Description,
            Category = clubRequest.Category,
            Status = "Approved",
            ReviewedBy = reviewedBy,
            ReviewedAt = DateTime.UtcNow,
            CreatedBy = clubRequest.RequestedBy,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Clubs.Add(club);

        clubRequest.Status = "Approved";
        clubRequest.ReviewedBy = reviewedBy;
        clubRequest.ReviewedAt = DateTime.UtcNow;

        _context.Notifications.Add(new Notification
        {
            UserId = clubRequest.RequestedBy,
            Title = "Club request approved",
            Message = "Your request for \"" + clubRequest.ProposedName + "\" was approved. The ICBT team will create your executive account shortly and notify credentials via email.",
            Type = "ClubRequest",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return MapToResponse(clubRequest);
    }

    public async Task<ClubRequestResponse?> RejectAsync(int clubRequestId, int reviewedBy, string reason)
    {
        var clubRequest = await _context.ClubRequests.FindAsync(clubRequestId);

        if (clubRequest == null)
        {
            return null;
        }

        clubRequest.Status = "Rejected";
        clubRequest.ReviewedBy = reviewedBy;
        clubRequest.ReviewedAt = DateTime.UtcNow;
        clubRequest.RejectionReason = reason;

        _context.Notifications.Add(new Notification
        {
            UserId = clubRequest.RequestedBy,
            Title = "Club request rejected",
            Message = "Your request for \"" + clubRequest.ProposedName + "\" was rejected. Reason: " + reason,
            Type = "ClubRequest",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return MapToResponse(clubRequest);
    }

    private async Task NotifyIcbtAdminsAsync(string title, string message)
    {
        var adminIds = await _context.Users
            .AsNoTracking()
            .Where(user => user.Role == "IcbtAdmin" && user.IsActive)
            .Select(user => user.UserId)
            .ToListAsync();

        var notificationTime = DateTime.UtcNow;

        foreach (var adminId in adminIds)
        {
            _context.Notifications.Add(new Notification
            {
                UserId = adminId,
                Title = title,
                Message = message,
                Type = "ClubRequest",
                IsRead = false,
                CreatedAt = notificationTime
            });
        }

        await _context.SaveChangesAsync();
    }

    private static ClubRequestResponse MapToResponse(ClubRequest clubRequest)
    {
        return new ClubRequestResponse
        {
            ClubRequestId = clubRequest.ClubRequestId,
            RequestedBy = clubRequest.RequestedBy,
            ProposedName = clubRequest.ProposedName,
            Category = clubRequest.Category,
            Description = clubRequest.Description,
            Purpose = clubRequest.Purpose,
            ExpectedMembers = clubRequest.ExpectedMembers,
            CoverLetter = clubRequest.CoverLetter,
            Status = clubRequest.Status,
            ReviewedBy = clubRequest.ReviewedBy,
            ReviewedAt = clubRequest.ReviewedAt,
            RejectionReason = clubRequest.RejectionReason,
            CreatedAt = clubRequest.CreatedAt
        };
    }
}