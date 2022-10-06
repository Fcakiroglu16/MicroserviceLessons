using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedEvents;
using WorkerService2.Models;

namespace WorkerService2.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    
    private static int stockNotUpdatedCount = 0;
    private static int stockUpdatedCount = 0;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<OrderCreatedEventConsumer> _logger;

    public OrderCreatedEventConsumer(AppDbContext context, ILogger<OrderCreatedEventConsumer> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var stockToUpdate =await  _dbContext.Stocks.FindAsync(context.Message.OrderId);

        if (stockToUpdate == null) return;
        stockToUpdate.Count = - context.Message.Count;
           await  _dbContext.SaveChangesAsync();
           stockUpdatedCount++;
            _logger.LogError("StockUpdatedCount :{Count}", stockUpdatedCount);
            _logger.LogError("Stock has decreased :{Count}, Order Sequence : {sequence}", stockToUpdate.Count,context
                .Message.OrderSequence);
        
   
       System.Threading.Thread.Sleep(500);
    }
}