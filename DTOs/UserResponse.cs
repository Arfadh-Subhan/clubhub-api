namespace ClubHub.DTOs;

public class UserResponse
{
    public int UserId { get; set; }
    public string? StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Semester { get; set; } = string.Empty;
    public byte[]? ProfilePhoto { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}