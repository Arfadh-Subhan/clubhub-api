using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class UpdatePositionRequest
{
    [Required]
    [MaxLength(50)]
    public string Position { get; set; } = string.Empty;
}