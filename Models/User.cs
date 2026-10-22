namespace ClubHub.Models;

public class User
{
    public int UserId { get; set; }
    public string? StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Semester { get; set; } = string.Empty;
    public byte[]? ProfilePhoto { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}