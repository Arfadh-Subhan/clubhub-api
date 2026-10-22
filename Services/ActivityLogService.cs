using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ActivityLogService : IActivityLogService
{
    private readonly AppDbContext _context;

    public ActivityLogService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ActivityLogResponse?> CreateAsync(CreateActivityLogRequest request)
    {
        var activityLog = new ActivityLog
        {
            Type = request.Type.Trim(),
            Message = request.Message.Trim(),
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.ActivityLogs.Add(activityLog);
        await _context.SaveChangesAsync();

        return MapToResponse(activityLog);
    }

    public async Task<List<ActivityLogResponse>?> GetAllAsync()
    {
        return await _context.ActivityLogs
            .AsNoTracking()
            .OrderByDescending(activityLog => activityLog.CreatedAt)
            .Select(activityLog => new ActivityLogResponse
            {
                ActivityLogId = activityLog.ActivityLogId,
                Type = activityLog.Type,
                Message = activityLog.Message,
                UserId = activityLog.UserId,
                CreatedAt = activityLog.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<ActivityLogResponse>?> GetByTypeAsync(string type)
    {
        return await _context.ActivityLogs
            .AsNoTracking()
            .Where(activityLog => activityLog.Type == type)
            .OrderByDescending(activityLog => activityLog.CreatedAt)
            .Select(activityLog => new ActivityLogResponse
            {
                ActivityLogId = activityLog.ActivityLogId,
                Type = activityLog.Type,
                Message = activityLog.Message,
                UserId = activityLog.UserId,
                CreatedAt = activityLog.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int activityLogId)
    {
        var activityLog = await _context.ActivityLogs.FindAsync(activityLogId);

        if (activityLog == null)
        {
            return false;
        }

        _context.ActivityLogs.Remove(activityLog);
        await _context.SaveChangesAsync();

        return true;
    }

    private static ActivityLogResponse MapToResponse(ActivityLog activityLog)
    {
        return new ActivityLogResponse
        {
            ActivityLogId = activityLog.ActivityLogId,
            Type = activityLog.Type,
            Message = activityLog.Message,
            UserId = activityLog.UserId,
            CreatedAt = activityLog.CreatedAt
        };
    }
}