using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class UpdateSiteReportStatusRequest
{
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}