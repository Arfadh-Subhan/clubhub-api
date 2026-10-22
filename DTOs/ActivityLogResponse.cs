namespace ClubHub.DTOs;

public class ActivityLogResponse
{
    public int ActivityLogId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}