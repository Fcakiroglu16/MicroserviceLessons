using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedEvents;
using WorkerService2.Models;

namespace WorkerService2.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private static int stockNotUpdatedCount;
    private static int stockUpdatedCount;
    private static int totalMessage;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<OrderCreatedEventConsumer> _logger;

    public OrderCreatedEventConsumer(AppDbContext context, ILogger<OrderCreatedEventConsumer> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var stockToUpdate = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
        await Task.Delay(300);
        if (stockToUpdate == null) return;
        totalMessage++;
        _logger.LogError(
            $"Total Message : {totalMessage},first record:{stockToUpdate.Count}, time:{DateTime.Now.ToString("hh:mm:ss.fff tt")}");
        try
        {
            stockToUpdate.Count = stockToUpdate.Count - context.Message.Count;
            await _dbContext.SaveChangesAsync();
            stockUpdatedCount++;

            _logger.LogError(
                "Stock has decreased :{Count}, Order Sequence : {sequence}, StockUpdatedCount :{StockUpdatedCount},time:{time}",
                stockToUpdate.Count, context
                    .Message.OrderSequence, stockUpdatedCount, DateTime.Now.ToString("hh:mm:ss.fff tt"));
        }
        catch (DbUpdateConcurrencyException e)
        {
            stockNotUpdatedCount++;
            _logger.LogCritical("stockNotUpdatedCount :{Count}, message :{message}", stockNotUpdatedCount, e.Message);
        }

        //System.Threading.Thread.Sleep(500);
    }
}