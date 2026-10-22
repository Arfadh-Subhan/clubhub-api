namespace ClubHub.Models;

public class ClubMeeting
{
    public int ClubMeetingId { get; set; }
    public int ClubId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime MeetingDate { get; set; }
    public string MeetingTime { get; set; } = string.Empty;
    public string? Links { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}