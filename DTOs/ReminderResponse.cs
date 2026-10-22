namespace ClubHub.DTOs;

public class ReminderResponse
{
    public int ReminderId { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public DateTime RemindAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
}