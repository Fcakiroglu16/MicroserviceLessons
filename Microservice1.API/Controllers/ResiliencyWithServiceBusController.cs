using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Microservice1.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResiliencyWithServiceBusController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ResiliencyWithServiceBusController(IPublishEndpoint publishEndpoinct)
    {
        _publishEndpoint = publishEndpoinct;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _publishEndpoint.Publish(new Event { Id = new Random().Next(1, 1000) });
        return Ok("The message has been sent to subscribers");
    }
}