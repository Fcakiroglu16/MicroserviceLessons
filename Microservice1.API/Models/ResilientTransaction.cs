using Microsoft.EntityFrameworkCore;

namespace Microservice1.API;

public class ResilientTransaction
{
    private readonly DbContext _context;

    private ResilientTransaction(DbContext context)
    {
        _context = context;
    }

    public static ResilientTransaction Create(DbContext context)
    {
        return new ResilientTransaction(context);
    }

    public async Task Execute(Func<Task> action)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            await action();
            await transaction.CommitAsync();
        });
    }
}