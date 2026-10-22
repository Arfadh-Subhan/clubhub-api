using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class RegisterRequest
{
    [Required]
    [MaxLength(20)]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    [ValidName]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [GmailOnly]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [PasswordStrength]
    public string Password { get; set; } = string.Empty;

    [Required]
    [CountryCode]
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Semester { get; set; } = string.Empty;
}