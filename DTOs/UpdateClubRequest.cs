using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class UpdateClubRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public byte[]? Logo { get; set; }

    public byte[]? CoverPhoto { get; set; }
}