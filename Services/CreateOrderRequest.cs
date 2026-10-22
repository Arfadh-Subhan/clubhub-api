using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class CreateOrderRequest
{
    [Required]
    public int ClubProductId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [MaxLength(20)]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
}
