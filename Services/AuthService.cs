using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        bool emailTaken = await _context.Users.AnyAsync(user => user.Email == normalizedEmail);

        if (emailTaken)
        {
            return null;
        }

        bool studentIdTaken = await _context.Users.AnyAsync(user => user.StudentId == request.StudentId);

        if (studentIdTaken)
        {
            return null;
        }

        var user = new User
        {
            StudentId = request.StudentId.Trim(),
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            PhoneNumber = request.PhoneNumber.Trim(),
            Semester = request.Semester.Trim(),
            Role = "Student",
            IsEmailVerified = false,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
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

    public async Task<UserResponse?> LoginAsync(LoginRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == normalizedEmail);

        if (user == null)
        {
            return null;
        }

        bool passwordMatches = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!passwordMatches)
        {
            return null;
        }

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

    public async Task<UserResponse?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(user => user.UserId == id)
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
}