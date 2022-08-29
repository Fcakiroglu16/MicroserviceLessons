using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;

namespace WebApplicationWithPublisher.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductsController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var productCreatedEvent = new ProductCreatedEvent { Id = 1, Name = "Pen 1", Price = 100 };
        var orderCreatedEvent = new OrderCreatedEvent { Id = 1, Name = "Mouse", Quantity = 20 };
        await _publishEndpoint.Publish(productCreatedEvent);
        await _publishEndpoint.Publish(orderCreatedEvent);
        return Ok("ProductCreatedEvent was send to message broker");
    }
}