using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class CreateActivityLogRequest
{
    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;

    public int? UserId { get; set; }
}