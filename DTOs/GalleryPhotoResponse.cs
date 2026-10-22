namespace ClubHub.DTOs;

public class GalleryPhotoResponse
{
    public int GalleryPhotoId { get; set; }
    public int ClubGalleryId { get; set; }
    public byte[] Photo { get; set; } = Array.Empty<byte>();
    public string? Caption { get; set; }
    public DateTime UploadedAt { get; set; }
}