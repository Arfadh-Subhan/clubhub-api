using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class CreateClubPostRequest
{
    [Required]
    public int ClubId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public byte[]? CoverImage { get; set; }

    [MaxLength(1000)]
    public string? Links { get; set; }

    [Required]
    public int CreatedBy { get; set; }
}