namespace ClubHub.DTOs;

public class ClubMemberResponse
{
    public int ClubMemberId { get; set; }
    public int ClubId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ClubRole { get; set; } = string.Empty;
    public string? Position { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime? JoinedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}