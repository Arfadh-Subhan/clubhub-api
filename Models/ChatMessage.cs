namespace ClubHub.Models;

public class ChatMessage
{
    public int ChatMessageId { get; set; }
    public int ClubId { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}