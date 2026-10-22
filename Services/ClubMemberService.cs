using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ClubMemberService : IClubMemberService
{
    private readonly AppDbContext _context;

    public ClubMemberService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClubMemberResponse?> JoinAsync(JoinClubRequest request)
    {
        var existing = await _context.ClubMembers
            .FirstOrDefaultAsync(clubMember => clubMember.ClubId == request.ClubId && clubMember.UserId == request.UserId);

        if (existing != null)
        {
            if (existing.Status == "Rejected")
            {
                existing.Status = "Pending";
                existing.ReviewedBy = null;
                existing.ReviewedAt = null;
                existing.JoinedAt = null;

                await _context.SaveChangesAsync();

                await NotifyClubExecutivesAsync(request.ClubId, request.UserId, "New join request", "A student requested to rejoin the club.");

                return await MapToResponseAsync(existing);
            }

            return null;
        }

        var member = new ClubMember
        {
            ClubId = request.ClubId,
            UserId = request.UserId,
            ClubRole = "Member",
            Position = null,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _context.ClubMembers.Add(member);
        await _context.SaveChangesAsync();

        await NotifyClubExecutivesAsync(request.ClubId, request.UserId, "New join request", "A student requested to join the club.");

        return await MapToResponseAsync(member);
    }

    public async Task<List<ClubMemberResponse>?> GetPendingByClubAsync(int clubId)
    {
        return await _context.ClubMembers
            .AsNoTracking()
            .Where(clubMember => clubMember.ClubId == clubId && clubMember.Status == "Pending")
            .Select(clubMember => new ClubMemberResponse
            {
                ClubMemberId = clubMember.ClubMemberId,
                ClubId = clubMember.ClubId,
                UserId = clubMember.UserId,
                FullName = _context.Users.Where(u => u.UserId == clubMember.UserId).Select(u => u.FullName).FirstOrDefault() ?? "Unknown User",
                ClubRole = clubMember.ClubRole,
                Position = clubMember.Position,
                Status = clubMember.Status,
                ReviewedBy = clubMember.ReviewedBy,
                ReviewedAt = clubMember.ReviewedAt,
                JoinedAt = clubMember.JoinedAt,
                CreatedAt = clubMember.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ClubMemberResponse?> ApproveAsync(int clubMemberId, int reviewedBy)
    {
        var member = await _context.ClubMembers.FirstOrDefaultAsync(clubMember => clubMember.ClubMemberId == clubMemberId);

        if (member == null)
        {
            return null;
        }

        member.Status = "Approved";
        member.ReviewedBy = reviewedBy;
        member.ReviewedAt = DateTime.UtcNow;
        member.JoinedAt = DateTime.UtcNow;

        var club = await _context.Clubs
            .AsNoTracking()
            .FirstOrDefaultAsync(club => club.ClubId == member.ClubId);

        string clubName = club != null ? club.Name : "the club";

        _context.Notifications.Add(new Notification
        {
            UserId = member.UserId,
            Title = "Join request approved",
            Message = "You are now a member of " + clubName + ".",
            Type = "ClubMember",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return await MapToResponseAsync(member);
    }

    public async Task<ClubMemberResponse?> RejectAsync(int clubMemberId, int reviewedBy)
    {
        var member = await _context.ClubMembers.FirstOrDefaultAsync(clubMember => clubMember.ClubMemberId == clubMemberId);

        if (member == null)
        {
            return null;
        }

        member.Status = "Rejected";
        member.ReviewedBy = reviewedBy;
        member.ReviewedAt = DateTime.UtcNow;

        var club = await _context.Clubs
            .AsNoTracking()
            .FirstOrDefaultAsync(club => club.ClubId == member.ClubId);

        string clubName = club != null ? club.Name : "the club";

        _context.Notifications.Add(new Notification
        {
            UserId = member.UserId,
            Title = "Join request rejected",
            Message = "Your request to join " + clubName + " was rejected.",
            Type = "ClubMember",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return await MapToResponseAsync(member);
    }

    public async Task<List<ClubMemberResponse>?> GetByClubAsync(int clubId)
    {
        return await _context.ClubMembers
            .AsNoTracking()
            .Where(clubMember => clubMember.ClubId == clubId && clubMember.Status == "Approved")
            .Select(clubMember => new ClubMemberResponse
            {
                ClubMemberId = clubMember.ClubMemberId,
                ClubId = clubMember.ClubId,
                UserId = clubMember.UserId,
                FullName = _context.Users.Where(u => u.UserId == clubMember.UserId).Select(u => u.FullName).FirstOrDefault() ?? "Unknown User",
                ClubRole = clubMember.ClubRole,
                Position = clubMember.Position,
                Status = clubMember.Status,
                ReviewedBy = clubMember.ReviewedBy,
                ReviewedAt = clubMember.ReviewedAt,
                JoinedAt = clubMember.JoinedAt,
                CreatedAt = clubMember.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<ClubMemberResponse>?> GetByUserAsync(int userId)
    {
        return await _context.ClubMembers
            .AsNoTracking()
            .Where(clubMember => clubMember.UserId == userId)
            .Select(clubMember => new ClubMemberResponse
            {
                ClubMemberId = clubMember.ClubMemberId,
                ClubId = clubMember.ClubId,
                UserId = clubMember.UserId,
                FullName = _context.Users.Where(u => u.UserId == clubMember.UserId).Select(u => u.FullName).FirstOrDefault() ?? "Unknown User",
                ClubRole = clubMember.ClubRole,
                Position = clubMember.Position,
                Status = clubMember.Status,
                ReviewedBy = clubMember.ReviewedBy,
                ReviewedAt = clubMember.ReviewedAt,
                JoinedAt = clubMember.JoinedAt,
                CreatedAt = clubMember.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ClubMemberResponse?> CreateExecutiveAsync(CreateExecutiveRequest request)
    {
        bool clubExists = await _context.Clubs.AnyAsync(club => club.ClubId == request.ClubId);

        if (!clubExists)
        {
            return null;
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        bool emailTaken = await _context.Users.AnyAsync(user => user.Email == normalizedEmail);

        if (emailTaken)
        {
            return null;
        }

        int existingPositionCount = await _context.ClubMembers
            .CountAsync(clubMember => clubMember.ClubId == request.ClubId && clubMember.Position == request.Position);

        int maxAllowed = 1;

        if (request.Position == "Vice President")
        {
            maxAllowed = 2;
        }
        else if (request.Position == "Academic Coordinator")
        {
            maxAllowed = 3;
        }
        else if (request.Position == "Non Academic Coordinator")
        {
            maxAllowed = 2;
        }

        if (existingPositionCount >= maxAllowed)
        {
            return null;
        }

        var user = new User
        {
            StudentId = null,
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            PhoneNumber = request.PhoneNumber.Trim(),
            Semester = "N/A",
            Role = "Executive",
            IsEmailVerified = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var member = new ClubMember
        {
            ClubId = request.ClubId,
            UserId = user.UserId,
            ClubRole = "Executive",
            Position = request.Position.Trim(),
            Status = "Approved",
            ReviewedAt = DateTime.UtcNow,
            JoinedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.ClubMembers.Add(member);
        await _context.SaveChangesAsync();

        return await MapToResponseAsync(member);
    }

    public async Task<ClubMemberResponse?> UpdatePositionAsync(int clubMemberId, string position)
    {
        var member = await _context.ClubMembers.FirstOrDefaultAsync(clubMember => clubMember.ClubMemberId == clubMemberId);

        if (member == null)
        {
            return null;
        }

        member.Position = position.Trim();

        await _context.SaveChangesAsync();

        return await MapToResponseAsync(member);
    }

    public async Task<bool> RemoveAsync(int clubMemberId)
    {
        var member = await _context.ClubMembers.FindAsync(clubMemberId);

        if (member == null)
        {
            return false;
        }

        var userId = member.UserId;

        var club = await _context.Clubs
            .AsNoTracking()
            .FirstOrDefaultAsync(club => club.ClubId == member.ClubId);

        string clubName = club != null ? club.Name : "the club";

        _context.ClubMembers.Remove(member);

        _context.Notifications.Add(new Notification
        {
            UserId = userId,
            Title = "Removed from club",
            Message = "You have been removed from " + clubName + ".",
            Type = "ClubMember",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return true;
    }

    private async Task NotifyClubExecutivesAsync(int clubId, int requesterUserId, string title, string message)
    {
        var executiveIds = await _context.ClubMembers
            .AsNoTracking()
            .Where(clubMember => clubMember.ClubId == clubId
                && clubMember.ClubRole == "Executive"
                && clubMember.Status == "Approved"
                && clubMember.UserId != requesterUserId)
            .Select(clubMember => clubMember.UserId)
            .ToListAsync();

        var notificationTime = DateTime.UtcNow;

        foreach (var executiveId in executiveIds)
        {
            _context.Notifications.Add(new Notification
            {
                UserId = executiveId,
                Title = title,
                Message = message,
                Type = "ClubMember",
                IsRead = false,
                CreatedAt = notificationTime
            });
        }

        await _context.SaveChangesAsync();
    }

    private async Task<ClubMemberResponse> MapToResponseAsync(ClubMember clubMember)
    {
        var fullName = await _context.Users
            .AsNoTracking()
            .Where(u => u.UserId == clubMember.UserId)
            .Select(u => u.FullName)
            .FirstOrDefaultAsync();

        return new ClubMemberResponse
        {
            ClubMemberId = clubMember.ClubMemberId,
            ClubId = clubMember.ClubId,
            UserId = clubMember.UserId,
            FullName = fullName ?? "Unknown User",
            ClubRole = clubMember.ClubRole,
            Position = clubMember.Position,
            Status = clubMember.Status,
            ReviewedBy = clubMember.ReviewedBy,
            ReviewedAt = clubMember.ReviewedAt,
            JoinedAt = clubMember.JoinedAt,
            CreatedAt = clubMember.CreatedAt
        };
    }
}