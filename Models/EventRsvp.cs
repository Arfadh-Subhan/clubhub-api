namespace ClubHub.Models;

public class EventRsvp
{
    public int EventRsvpId { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}