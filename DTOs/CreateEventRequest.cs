using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class CreateEventRequest
{
    public int? ClubId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime EventDate { get; set; }

    [Required]
    [MaxLength(20)]
    public string EventTime { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Venue { get; set; } = string.Empty;

    public byte[]? Poster { get; set; }

    [MaxLength(1000)]
    public string? Links { get; set; }

    public int? Capacity { get; set; }

    public decimal? EstimatedBudget { get; set; }

    public byte[]? PermissionProof { get; set; }

    [Required]
    public int CreatedBy { get; set; }
}