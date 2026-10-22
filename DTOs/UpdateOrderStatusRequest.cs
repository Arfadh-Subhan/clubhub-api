using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class UpdateOrderStatusRequest
{
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}