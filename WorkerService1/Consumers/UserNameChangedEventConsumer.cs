using MassTransit;
using RedLockNet.SERedis;
using SharedEvents;
using WorkerService1.Models;

namespace WorkerService1.Consumers;

public class UserNameChangedEventConsumer : IConsumer<UserNameChangedEvent>
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<UserNameChangedEventConsumer> _logger;
    private readonly RedLockFactory _redLockFactory;

    public UserNameChangedEventConsumer(AppDbContext dbContext, ILogger<UserNameChangedEventConsumer> logger, RedLockFactory redLockFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _redLockFactory = redLockFactory;
    }

    public async Task Consume(ConsumeContext<UserNameChangedEvent> context)
    { 
        
        var expiry = TimeSpan.FromSeconds(30);
        await using (var redLock = await _redLockFactory.CreateLockAsync(context.Message.Id.ToString(), expiry))
        {
            if (redLock.IsAcquired)
            {
                var userToUpdate = await _dbContext.Users.FindAsync(context.Message.Id);

                if (userToUpdate == null) return;
        
                if (!userToUpdate.UpdatedDateTime.HasValue)
                {
                    userToUpdate.UserName = context.Message.UserName;
                    userToUpdate.UpdatedDateTime = context.Message.CreatedDateTime.ToUniversalTime();
                    await _dbContext.SaveChangesAsync();
                    _logger.LogError("UserName has changed :{UserName}", userToUpdate.UserName);
                    return;
                }

                if (context.Message.CreatedDateTime > userToUpdate.UpdatedDateTime)
                {
                    userToUpdate.UserName = context.Message.UserName;
                    userToUpdate.UpdatedDateTime = context.Message.CreatedDateTime.ToUniversalTime();
                    await _dbContext.SaveChangesAsync();
                    _logger.LogError("UserName has changed :{UserName}", userToUpdate.UserName);

                }
                else
                {
                    _logger.LogCritical("UserName has't changed :{UserName}", userToUpdate.UserName);
            
                }
            
            
            }
            else
            {
                
                
                _logger.LogCritical("Locked");
             
             //   throw new Exception("Lock situation created");
            }
        }
       
        
    
        
     

        Thread.Sleep(1000);
    }
}