using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class CreateGalleryPhotoRequest
{
    [Required]
    public int ClubGalleryId { get; set; }

    [Required]
    public byte[] Photo { get; set; } = Array.Empty<byte>();

    [MaxLength(200)]
    public string? Caption { get; set; }
}