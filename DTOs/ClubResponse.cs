namespace ClubHub.DTOs;

public class ClubResponse
{
    public int ClubId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public byte[]? Logo { get; set; }
    public byte[]? CoverPhoto { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? RejectionReason { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}