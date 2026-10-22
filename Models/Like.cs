namespace ClubHub.Models;

public class Like
{
    public int LikeId { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}