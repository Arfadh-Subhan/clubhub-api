using ClubHub.DTOs;

namespace ClubHub.Services;

public interface ISiteReportService
{
    Task<SiteReportResponse?> CreateAsync(CreateSiteReportRequest request);
    Task<List<SiteReportResponse>?> GetAllAsync();
    Task<SiteReportResponse?> GetByIdAsync(int siteReportId);
    Task<SiteReportResponse?> UpdateStatusAsync(int siteReportId, string status);
    Task<bool> DeleteAsync(int siteReportId);
}