using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IActivityLogService
{
    Task<ActivityLogResponse?> CreateAsync(CreateActivityLogRequest request);
    Task<List<ActivityLogResponse>?> GetAllAsync();
    Task<List<ActivityLogResponse>?> GetByTypeAsync(string type);
    Task<bool> DeleteAsync(int activityLogId);
}