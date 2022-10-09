using System.Net;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using WorkerService2;
using WorkerService2.Consumers;
using WorkerService2.Models;

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

await host.RunAsync();