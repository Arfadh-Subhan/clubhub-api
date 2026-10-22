namespace ClubHub.Models;

public class Reminder
{
    public int ReminderId { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public DateTime RemindAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
