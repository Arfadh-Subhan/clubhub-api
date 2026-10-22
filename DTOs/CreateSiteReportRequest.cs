using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class CreateSiteReportRequest
{
    public int? UserId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;
}