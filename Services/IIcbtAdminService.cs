using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IIcbtAdminService
{
    Task<PageResult<IcbtAdminUserResponse>?> GetAllUsersAsync(int pageNumber, int pageSize);
    Task<IcbtAdminUserResponse?> GetUserByIdAsync(int userId);
    Task<IcbtAdminUserResponse?> CreateUserAsync(SiteAdminCreateUserRequest request);
    Task<IcbtAdminUserResponse?> UpdateUserAsync(int userId, IcbtAdminUpdateUserRequest request);
    Task<IcbtAdminUserResponse?> ResetPasswordAsync(int userId, IcbtAdminResetPasswordRequest request);
    Task<IcbtAdminUserResponse?> SuspendUserAsync(int userId);
    Task<IcbtAdminUserResponse?> ActivateUserAsync(int userId);
    Task<bool> DeleteUserAsync(int userId);
}