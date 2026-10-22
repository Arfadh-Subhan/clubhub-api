using ClubHub.Data;
using ClubHub.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ProfileService : IProfileService
{
    private readonly AppDbContext _context;

    public ProfileService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserResponse?> GetProfileAsync(int userId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(user => user.UserId == userId)
            .Select(user => new UserResponse
            {
                UserId = user.UserId,
                StudentId = user.StudentId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Semester = user.Semester,
                ProfilePhoto = user.ProfilePhoto,
                Role = user.Role,
                IsEmailVerified = user.IsEmailVerified,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UserResponse?> UpdateProfileAsync(int userId, UpdateProfileRequest request)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return null;
        }

        user.FullName = request.FullName.Trim();
        user.PhoneNumber = request.PhoneNumber.Trim();
        user.Semester = request.Semester.Trim();

        if (request.ProfilePhoto != null)
        {
            user.ProfilePhoto = request.ProfilePhoto;
        }

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new UserResponse
        {
            UserId = user.UserId,
            StudentId = user.StudentId,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Semester = user.Semester,
            ProfilePhoto = user.ProfilePhoto,
            Role = user.Role,
            IsEmailVerified = user.IsEmailVerified,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}