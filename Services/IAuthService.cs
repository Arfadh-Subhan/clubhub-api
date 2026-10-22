using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IAuthService
{
    Task<UserResponse?> RegisterAsync(RegisterRequest request);
    Task<UserResponse?> LoginAsync(LoginRequest request);
    Task<UserResponse?> GetUserByIdAsync(int id);
}