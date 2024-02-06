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
    public async Task<IActionResult> Get(CancellationToken token)
    {
        //try
        //{
        //    using var source = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        //    await _publishEndpoint.Publish(new Event { Id = new Random().Next(1, 1000) }, source.Token);
        //}
        //catch (Exception e)
        //{

        //    throw;
        //}
        var id = new Random().Next(1, 1000);
        using var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
       await _publishEndpoint.Publish(new Event { Id = id }, callback: (context) =>
        {
            context.SetAwaitAck(true);
            context.Durable = true;
            context.CorrelationId = NewId.NextGuid();
            context.Headers.Set("key", "value");
           
        },source.Token);
        return Ok($"The message has been sent to subscribers: {id}");
    }
}