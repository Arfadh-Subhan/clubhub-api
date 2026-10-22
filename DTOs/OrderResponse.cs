namespace ClubHub.DTOs;

public class OrderResponse
{
    public int OrderId { get; set; }
    public int ClubProductId { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}