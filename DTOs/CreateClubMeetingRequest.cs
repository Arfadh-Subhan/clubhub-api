using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class CreateClubMeetingRequest
{
    [Required]
    public int ClubId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    [Required]
    public DateTime MeetingDate { get; set; }

    [Required]
    [MaxLength(20)]
    public string MeetingTime { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Links { get; set; }

    [Required]
    public int CreatedBy { get; set; }
}