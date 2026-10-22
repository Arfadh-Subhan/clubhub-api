using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class CreateReminderRequest
{
    [Required]
    public int EventId { get; set; }

    [Required]
    public int UserId { get; set; }

    [MaxLength(200)]
    public string? Message { get; set; }
}