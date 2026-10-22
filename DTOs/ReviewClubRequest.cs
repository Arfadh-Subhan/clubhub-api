using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class ReviewClubRequest
{
    [Required]
    public int ReviewedBy { get; set; }

    [MaxLength(500)]
    public string? RejectionReason { get; set; }
}