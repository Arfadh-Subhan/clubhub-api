using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IClubService
{
    Task<ClubResponse?> CreateAsync(CreateClubRequest request);
    Task<PageResult<ClubResponse>?> GetAllAsync(int pageNumber, int pageSize);
    Task<ClubResponse?> GetByIdAsync(int id);
    Task<ClubResponse?> UpdateAsync(int id, UpdateClubRequest request);
    Task<bool> DeleteAsync(int id);
}