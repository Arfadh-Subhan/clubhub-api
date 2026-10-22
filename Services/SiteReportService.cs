using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class SiteReportService : ISiteReportService
{
    private readonly AppDbContext _context;

    public SiteReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SiteReportResponse?> CreateAsync(CreateSiteReportRequest request)
    {
        var siteReport = new SiteReport
        {
            UserId = request.UserId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Status = "Open",
            CreatedAt = DateTime.UtcNow
        };

        _context.SiteReports.Add(siteReport);
        await _context.SaveChangesAsync();

        return MapToResponse(siteReport);
    }

    public async Task<List<SiteReportResponse>?> GetAllAsync()
    {
        return await _context.SiteReports
            .AsNoTracking()
            .OrderByDescending(siteReport => siteReport.CreatedAt)
            .Select(siteReport => new SiteReportResponse
            {
                SiteReportId = siteReport.SiteReportId,
                UserId = siteReport.UserId,
                Title = siteReport.Title,
                Description = siteReport.Description,
                Status = siteReport.Status,
                CreatedAt = siteReport.CreatedAt,
                UpdatedAt = siteReport.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<SiteReportResponse?> GetByIdAsync(int siteReportId)
    {
        return await _context.SiteReports
            .AsNoTracking()
            .Where(siteReport => siteReport.SiteReportId == siteReportId)
            .Select(siteReport => new SiteReportResponse
            {
                SiteReportId = siteReport.SiteReportId,
                UserId = siteReport.UserId,
                Title = siteReport.Title,
                Description = siteReport.Description,
                Status = siteReport.Status,
                CreatedAt = siteReport.CreatedAt,
                UpdatedAt = siteReport.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<SiteReportResponse?> UpdateStatusAsync(int siteReportId, string status)
    {
        var siteReport = await _context.SiteReports.FindAsync(siteReportId);

        if (siteReport == null)
        {
            return null;
        }

        siteReport.Status = status.Trim();
        siteReport.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(siteReport);
    }

    public async Task<bool> DeleteAsync(int siteReportId)
    {
        var siteReport = await _context.SiteReports.FindAsync(siteReportId);

        if (siteReport == null)
        {
            return false;
        }

        _context.SiteReports.Remove(siteReport);
        await _context.SaveChangesAsync();

        return true;
    }

    private static SiteReportResponse MapToResponse(SiteReport siteReport)
    {
        return new SiteReportResponse
        {
            SiteReportId = siteReport.SiteReportId,
            UserId = siteReport.UserId,
            Title = siteReport.Title,
            Description = siteReport.Description,
            Status = siteReport.Status,
            CreatedAt = siteReport.CreatedAt,
            UpdatedAt = siteReport.UpdatedAt
        };
    }
}