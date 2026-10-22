using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IProfileService
{
    Task<UserResponse?> GetProfileAsync(int userId);
    Task<UserResponse?> UpdateProfileAsync(int userId, UpdateProfileRequest request);
}