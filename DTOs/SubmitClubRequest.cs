using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class SubmitClubRequest
{
    [Required]
    public int RequestedBy { get; set; }

    [Required]
    [MaxLength(100)]
    public string ProposedName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Purpose { get; set; } = string.Empty;

    public int? ExpectedMembers { get; set; }

    [Required]
    public byte[] CoverLetter { get; set; } = Array.Empty<byte>();
}