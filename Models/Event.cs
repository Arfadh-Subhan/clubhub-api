namespace ClubHub.Models;

public class Event
{
    public int EventId { get; set; }
    public int? ClubId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string EventTime { get; set; } = string.Empty;
    public DateTime EventStartAt { get; set; }
    public string Venue { get; set; } = string.Empty;
    public byte[]? Poster { get; set; }
    public string? Links { get; set; }
    public int? Capacity { get; set; }
    public decimal? EstimatedBudget { get; set; }
    public byte[]? PermissionProof { get; set; }
    public string Status { get; set; } = "Pending";
    public int? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? RejectionReason { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}