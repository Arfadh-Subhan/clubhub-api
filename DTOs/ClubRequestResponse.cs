namespace ClubHub.DTOs;

public class ClubRequestResponse
{
    public int ClubRequestId { get; set; }
    public int RequestedBy { get; set; }
    public string ProposedName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public int? ExpectedMembers { get; set; }
    public byte[]? CoverLetter { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
}