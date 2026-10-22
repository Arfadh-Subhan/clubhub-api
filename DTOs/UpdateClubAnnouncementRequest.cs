using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class UpdateClubAnnouncementRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Links { get; set; }
}