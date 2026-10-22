namespace ClubHub.Models;

public class ClubMember
{
    public int ClubMemberId { get; set; }
    public int ClubId { get; set; }
    public int UserId { get; set; }
    public string ClubRole { get; set; } = "Member";
    public string? Position { get; set; }
    public string Status { get; set; } = "Pending";
    public int? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime? JoinedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}