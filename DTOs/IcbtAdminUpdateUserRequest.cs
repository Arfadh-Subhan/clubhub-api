using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class IcbtAdminUpdateUserRequest
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [GmailOnly]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [CountryCode]
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Semester { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = string.Empty;
}