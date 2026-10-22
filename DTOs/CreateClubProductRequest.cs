using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class CreateClubProductRequest
{
    [Required]
    public int ClubId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    public byte[]? Image { get; set; }

    [Required]
    public int Stock { get; set; }

    [Required]
    public int CreatedBy { get; set; }
}