using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class JoinClubRequest
{
    [Required]
    public int ClubId { get; set; }

    [Required]
    public int UserId { get; set; }
}