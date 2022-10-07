using MassTransit;
using Microsoft.EntityFrameworkCore;
using RedLockNet.SERedis;
using SharedEvents;
using WorkerService1.Models;

namespace WorkerService1.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private static int _lockedCount;
    private static int _stockUpdatedCount;
    private static int _totalMessage;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private readonly RedLockFactory _redLockFactory;

    public OrderCreatedEventConsumer(AppDbContext context, ILogger<OrderCreatedEventConsumer> logger,
        RedLockFactory redLockFactory)
    {
        _dbContext = context;
        _logger = logger;
        _redLockFactory = redLockFactory;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        _totalMessage++;
        var expiry = TimeSpan.FromSeconds(30);
        await using (var redLock = await _redLockFactory.CreateLockAsync(context.Message.OrderId.ToString(), expiry))
        {
            // make sure we got the lock
            if (redLock.IsAcquired)
            {
                var stockToUpdate = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
                await Task.Delay(400);
                if (stockToUpdate == null) return;

                stockToUpdate.Count = stockToUpdate.Count - context.Message.Count;
                await _dbContext.SaveChangesAsync();
                _stockUpdatedCount++;

                _logger.LogInformation(
                    "Stock has decreased :{Count}, Order Sequence : {Sequence}, StockUpdatedCount :{StockUpdatedCount},time:{Time}",
                    stockToUpdate.Count, context
                        .Message.OrderSequence, _stockUpdatedCount, DateTime.Now.ToString("hh:mm:ss.fff tt"));
            }
            else
            {
                _lockedCount++;
                _logger.LogError("Locked Count :{LockedCount}", _lockedCount);
            }
        }

        await Task.Delay(700);
    }
}