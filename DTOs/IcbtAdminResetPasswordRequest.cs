using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class IcbtAdminResetPasswordRequest
{
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string NewPassword { get; set; } = string.Empty;
}