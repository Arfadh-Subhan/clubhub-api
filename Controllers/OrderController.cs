using ClubHub.DTOs;
using ClubHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClubHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var result = await _orderService.CreateAsync(request);

        if (result == null)
        {
            return BadRequest(new { message = "Could not place the order. Product not available or insufficient stock!" });
        }

        return Ok(new { message = "Order placed successfully!", Order = result });
    }

    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> GetById(int orderId)
    {
        var result = await _orderService.GetByIdAsync(orderId);

        if (result == null)
        {
            return NotFound(new { message = "Order not found!" });
        }

        return Ok(result);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var result = await _orderService.GetByUserAsync(userId);

        return Ok(result);
    }

    [HttpGet("product/{clubProductId:int}")]
    public async Task<IActionResult> GetByProduct(int clubProductId)
    {
        var result = await _orderService.GetByProductAsync(clubProductId);

        return Ok(result);
    }

    [HttpPut("{orderId:int}/status")]
    public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] UpdateOrderStatusRequest request)
    {
        var result = await _orderService.UpdateStatusAsync(orderId, request.Status);

        if (result == null)
        {
            return NotFound(new { message = "Order not found!" });
        }

        return Ok(new { message = "Order status updated!", Order = result });
    }
}