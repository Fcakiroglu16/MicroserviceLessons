using MassTransit;
using Microsoft.EntityFrameworkCore;
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

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedEventConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("order-create-event-queue", e =>
                {
                    e.ConcurrentMessageLimit = 1;
                    e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
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

    dbContext.Stocks.Add(new Stock { Id = 1, Name = "Pen 1", Count = 50 });
    dbContext.SaveChanges();
}

await host.RunAsync();