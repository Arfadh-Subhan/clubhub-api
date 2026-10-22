using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class CreateChatMessageRequest
{
    [Required]
    public int ClubId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = string.Empty;
}