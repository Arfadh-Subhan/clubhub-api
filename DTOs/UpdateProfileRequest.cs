using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class UpdateProfileRequest
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [CountryCode]
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Semester { get; set; } = string.Empty;

    public byte[]? ProfilePhoto { get; set; }
}