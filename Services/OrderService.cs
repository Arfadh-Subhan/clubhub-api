using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResponse?> CreateAsync(CreateOrderRequest request)
    {
        var clubProduct = await _context.ClubProducts.FindAsync(request.ClubProductId);

        if (clubProduct == null)
        {
            return null;
        }

        if (clubProduct.Status != "Approved" || !clubProduct.IsActive)
        {
            return null;
        }

        if (clubProduct.Stock < request.Quantity)
        {
            return null;
        }

        var order = new Order
        {
            ClubProductId = request.ClubProductId,
            UserId = request.UserId,
            Quantity = request.Quantity,
            StudentId = request.StudentId.Trim(),
            FullName = request.FullName.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            TotalPrice = clubProduct.Price * request.Quantity,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        clubProduct.Stock = clubProduct.Stock - request.Quantity;
        clubProduct.UpdatedAt = DateTime.UtcNow;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return MapToResponse(order);
    }

    public async Task<OrderResponse?> GetByIdAsync(int orderId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(order => order.OrderId == orderId)
            .Select(order => new OrderResponse
            {
                OrderId = order.OrderId,
                ClubProductId = order.ClubProductId,
                UserId = order.UserId,
                Quantity = order.Quantity,
                StudentId = order.StudentId,
                FullName = order.FullName,
                PhoneNumber = order.PhoneNumber,
                Email = order.Email,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<OrderResponse>?> GetByUserAsync(int userId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(order => order.UserId == userId)
            .OrderByDescending(order => order.CreatedAt)
            .Select(order => new OrderResponse
            {
                OrderId = order.OrderId,
                ClubProductId = order.ClubProductId,
                UserId = order.UserId,
                Quantity = order.Quantity,
                StudentId = order.StudentId,
                FullName = order.FullName,
                PhoneNumber = order.PhoneNumber,
                Email = order.Email,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<List<OrderResponse>?> GetByProductAsync(int clubProductId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(order => order.ClubProductId == clubProductId)
            .OrderByDescending(order => order.CreatedAt)
            .Select(order => new OrderResponse
            {
                OrderId = order.OrderId,
                ClubProductId = order.ClubProductId,
                UserId = order.UserId,
                Quantity = order.Quantity,
                StudentId = order.StudentId,
                FullName = order.FullName,
                PhoneNumber = order.PhoneNumber,
                Email = order.Email,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<OrderResponse?> UpdateStatusAsync(int orderId, string status)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order == null)
        {
            return null;
        }

        order.Status = status.Trim();
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(order);
    }

    private static OrderResponse MapToResponse(Order order)
    {
        return new OrderResponse
        {
            OrderId = order.OrderId,
            ClubProductId = order.ClubProductId,
            UserId = order.UserId,
            Quantity = order.Quantity,
            StudentId = order.StudentId,
            FullName = order.FullName,
            PhoneNumber = order.PhoneNumber,
            Email = order.Email,
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}