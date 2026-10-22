namespace ClubHub.DTOs;

public class EventRsvpResponse
{
    public int EventRsvpId { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}