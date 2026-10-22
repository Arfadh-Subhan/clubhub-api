namespace ClubHub.DTOs;

public class ClubAnnouncementResponse
{
    public int ClubAnnouncementId { get; set; }
    public int ClubId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Links { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? RejectionReason { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
