namespace ClubHub.DTOs;

public class ClubGalleryResponse
{
    public int ClubGalleryId { get; set; }
    public int ClubId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public byte[]? CoverPhoto { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}