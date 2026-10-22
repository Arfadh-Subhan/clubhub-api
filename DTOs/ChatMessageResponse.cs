namespace ClubHub.DTOs;

public class ChatMessageResponse
{
    public int ChatMessageId { get; set; }
    public int ClubId { get; set; }
    public int UserId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderPosition { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
}