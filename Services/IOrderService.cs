using ClubHub.DTOs;

namespace ClubHub.Services;

public interface IOrderService
{
    Task<OrderResponse?> CreateAsync(CreateOrderRequest request);
    Task<OrderResponse?> GetByIdAsync(int orderId);
    Task<List<OrderResponse>?> GetByUserAsync(int userId);
    Task<List<OrderResponse>?> GetByProductAsync(int clubProductId);
    Task<OrderResponse?> UpdateStatusAsync(int orderId, string status);
}