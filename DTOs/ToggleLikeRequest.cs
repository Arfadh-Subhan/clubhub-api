using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class ToggleLikeRequest
{
    [Required]
    public int EventId { get; set; }

    [Required]
    public int UserId { get; set; }
}