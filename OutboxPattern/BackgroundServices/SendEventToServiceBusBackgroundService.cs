using System.Text.Json;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OutboxPattern.Events;
using OutboxPattern.Models;

namespace OutboxPattern.BackgroundServices;

public class SendEventToServiceBusBackgroundService : BackgroundService
{
    private readonly ILogger<SendEventToServiceBusBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public SendEventToServiceBusBackgroundService(IServiceProvider serviceProvider,
        ILogger<SendEventToServiceBusBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var sendEndpointProvider = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:order-queue"));
            var outboxs = await context.Outboxes.Where(x => x.IsSendBus == false).OrderBy(x => x.Created).Take(100)
                .ToListAsync(stoppingToken);
            foreach (var outbox in outboxs)
            {
                var orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(outbox.Payload);
                await sendEndpoint.Send(orderCreatedEvent, stoppingToken);
                outbox.IsSendBus = true;
                _logger.LogInformation("orderCreatedEvent has been send to queue: Id : {Id}", orderCreatedEvent!.Id);
            }

            await context.SaveChangesAsync(stoppingToken);

            await Task.Delay(2000, stoppingToken);
        }
    }
}