using System.Net;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using WorkerService1;
using WorkerService1.Consumers;
using WorkerService1.Models;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((contextHost, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(contextHost.Configuration.GetConnectionString("PostgreSQL"));
        });

        services.AddHostedService<Worker>();
        services.AddSingleton<RedLockFactory>(sp =>
        {
            var endPoints = new List<RedLockEndPoint>
            {
                new DnsEndPoint("localhost", 6379)
            };
            return RedLockFactory.Create(endPoints);
        });
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserNameChangedEventConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("username-changed-event-queue", e =>
                {
                    e.ConcurrentMessageLimit = 1;
                    e.ConfigureConsumer<UserNameChangedEventConsumer>(context);
                });
            });
        });
    })
    .Build();
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();

    dbContext.Users.Add(new User { UserName = "jone", Email = "jon@outlook.com", Age = 20 ,CreatedDateTime = DateTime
    .Now.ToUniversalTime()});
    dbContext.SaveChanges();
}

await host.RunAsync();