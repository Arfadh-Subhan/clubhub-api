using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class CreateClubGalleryRequest
{
    [Required]
    public int ClubId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public byte[]? CoverPhoto { get; set; }

    [Required]
    public int CreatedBy { get; set; }
}
