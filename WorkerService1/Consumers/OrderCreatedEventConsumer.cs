using MassTransit;
using SharedEvents;
using WorkerService1.Models;

namespace WorkerService1.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<OrderCreatedEventConsumer> _logger;

    public OrderCreatedEventConsumer(AppDbContext context, ILogger<OrderCreatedEventConsumer> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var stockToUpdate = await _dbContext.Stocks.FindAsync(context.Message.OrderId);

        if (stockToUpdate == null) return;
        stockToUpdate.Count = stockToUpdate.Count - context.Message.Count;
        await _dbContext.SaveChangesAsync();
        _logger.LogError("Stock has decreased :{Count}, Order Sequence : {Sequence}", stockToUpdate.Count, context
            .Message.OrderSequence);

        Thread.Sleep(300);
    }
}