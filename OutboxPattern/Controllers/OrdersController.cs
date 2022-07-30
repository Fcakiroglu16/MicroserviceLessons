using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OutboxPattern.Events;
using OutboxPattern.Models;

namespace OutboxPattern.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var order = new Order { Name = "Pen", Price = 100, Count = 1 };

        await _context.Orders.AddAsync(order);

        await _context.SaveChangesAsync();
        var orderCreatedEvent = new OrderCreatedEvent
        {
            Name = order.Name,
            Price = order.Price,
            Count = order.Count,
            Id = order.Id
        };

        await _context.Outboxes.AddAsync(new Outbox
        {
            Payload = JsonSerializer.Serialize(orderCreatedEvent), Created = DateTime.UtcNow, IsSendBus =
                false
        });

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return Ok();
    }
}