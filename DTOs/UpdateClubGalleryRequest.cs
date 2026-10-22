using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class UpdateClubGalleryRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public byte[]? CoverPhoto { get; set; }
}