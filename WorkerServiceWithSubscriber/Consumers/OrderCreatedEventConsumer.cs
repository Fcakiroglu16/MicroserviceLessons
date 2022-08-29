using MassTransit;
using SharedLibrary;
using WorkerServiceWithSubscriber.Models;

namespace WorkerServiceWithSubscriber.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly AppDbContext _context;

    public OrderCreatedEventConsumer(AppDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        if (!_context.QueueIdempotentInboxes.Any(x => x.MessageId.Equals(context.MessageId)))
        {
            QueueIdempotentInbox queueIdempotentInbox = new()
            {
                MessageId = context.MessageId!.Value,
                Consumer = GetType().Name,
                Created = DateTime.Now
            };

            // await using var transaction = await _context.Database.BeginTransactionAsync();
            await _context.QueueIdempotentInboxes.AddAsync(queueIdempotentInbox);
            await _context.SaveChangesAsync();
            // await transaction.CommitAsync();
            Console.WriteLine("OrderCreatedEventConsumer worked");
        }
    }
}