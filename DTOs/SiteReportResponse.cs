namespace ClubHub.DTOs;

public class SiteReportResponse
{
    public int SiteReportId { get; set; }
    public int? UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}