using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class IcbtAdminService : IIcbtAdminService
{
    private readonly AppDbContext _context;

    public IcbtAdminService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PageResult<IcbtAdminUserResponse>?> GetAllUsersAsync(int pageNumber, int pageSize)
    {
        var query = _context.Users
            .AsNoTracking()
            .OrderBy(user => user.UserId);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(user => new IcbtAdminUserResponse
            {
                UserId = user.UserId,
                StudentId = user.StudentId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Semester = user.Semester,
                Role = user.Role,
                IsEmailVerified = user.IsEmailVerified,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            })
            .ToListAsync();

        return new PageResult<IcbtAdminUserResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<IcbtAdminUserResponse?> GetUserByIdAsync(int userId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(user => user.UserId == userId)
            .Select(user => new IcbtAdminUserResponse
            {
                UserId = user.UserId,
                StudentId = user.StudentId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Semester = user.Semester,
                Role = user.Role,
                IsEmailVerified = user.IsEmailVerified,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IcbtAdminUserResponse?> CreateUserAsync(SiteAdminCreateUserRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        bool emailTaken = await _context.Users
            .AnyAsync(user => user.Email == normalizedEmail);

        if (emailTaken)
        {
            return null;
        }

        string? studentId = null;
        string semester = "N/A";
        bool isEmailVerified = true;

        if (request.Role == "Student")
        {
            if (string.IsNullOrWhiteSpace(request.StudentId))
            {
                return null;
            }

            var normalizedStudentId = request.StudentId.Trim();

            bool studentIdTaken = await _context.Users
                .AnyAsync(user => user.StudentId == normalizedStudentId);

            if (studentIdTaken)
            {
                return null;
            }

            studentId = normalizedStudentId;

            if (!string.IsNullOrWhiteSpace(request.Semester))
            {
                semester = request.Semester.Trim();
            }

            isEmailVerified = false;
        }

        var user = new User
        {
            StudentId = studentId,
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            PhoneNumber = request.PhoneNumber.Trim(),
            Semester = semester,
            Role = request.Role.Trim(),
            IsEmailVerified = isEmailVerified,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return MapToResponse(user);
    }

    public async Task<IcbtAdminUserResponse?> UpdateUserAsync(int userId, IcbtAdminUpdateUserRequest request)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return null;
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        bool emailTaken = await _context.Users
            .AnyAsync(otherUser => otherUser.Email == normalizedEmail && otherUser.UserId != userId);

        if (emailTaken)
        {
            return null;
        }

        user.FullName = request.FullName.Trim();
        user.Email = normalizedEmail;
        user.PhoneNumber = request.PhoneNumber.Trim();
        user.Semester = request.Semester.Trim();
        user.Role = request.Role.Trim();
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(user);
    }

    public async Task<IcbtAdminUserResponse?> ResetPasswordAsync(int userId, IcbtAdminResetPasswordRequest request)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return null;
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(user);
    }

    public async Task<IcbtAdminUserResponse?> SuspendUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return null;
        }

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(user);
    }

    public async Task<IcbtAdminUserResponse?> ActivateUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return null;
        }

        user.IsActive = true;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(user);
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return false;
        }

        var clubMembers = await _context.ClubMembers
            .Where(clubMember => clubMember.UserId == userId)
            .ToListAsync();
        _context.ClubMembers.RemoveRange(clubMembers);

        var orders = await _context.Orders
            .Where(order => order.UserId == userId)
            .ToListAsync();
        _context.Orders.RemoveRange(orders);

        var eventRsvps = await _context.EventRsvps
            .Where(eventRsvp => eventRsvp.UserId == userId)
            .ToListAsync();
        _context.EventRsvps.RemoveRange(eventRsvps);

        var reminders = await _context.Reminders
            .Where(reminder => reminder.UserId == userId)
            .ToListAsync();
        _context.Reminders.RemoveRange(reminders);

        var notifications = await _context.Notifications
            .Where(notification => notification.UserId == userId)
            .ToListAsync();
        _context.Notifications.RemoveRange(notifications);

        var chatMessages = await _context.ChatMessages
            .Where(chatMessage => chatMessage.UserId == userId)
            .ToListAsync();
        _context.ChatMessages.RemoveRange(chatMessages);

        var siteReports = await _context.SiteReports
            .Where(siteReport => siteReport.UserId == userId)
            .ToListAsync();

        foreach (var siteReport in siteReports)
        {
            siteReport.UserId = null;
        }

        var clubRequests = await _context.ClubRequests
            .Where(clubRequest => clubRequest.RequestedBy == userId)
            .ToListAsync();

        foreach (var clubRequest in clubRequests)
        {
            clubRequest.RequestedBy = 0;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    private static IcbtAdminUserResponse MapToResponse(User user)
    {
        return new IcbtAdminUserResponse
        {
            UserId = user.UserId,
            StudentId = user.StudentId,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Semester = user.Semester,
            Role = user.Role,
            IsEmailVerified = user.IsEmailVerified,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}