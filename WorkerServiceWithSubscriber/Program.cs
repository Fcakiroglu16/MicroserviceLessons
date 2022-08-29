using MassTransit;
using Microsoft.EntityFrameworkCore;
using WorkerServiceWithSubscriber;
using WorkerServiceWithSubscriber.Consumers;
using WorkerServiceWithSubscriber.Models;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<AppDbContext>(option => option.UseInMemoryDatabase("mydatabase"));
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProductCreatedEvetConsumer>();
            x.AddConsumer<OrderCreatedEventConsumer>();
            x.UsingRabbitMq((context, config) =>
            {
                config.ReceiveEndpoint("queue-order-create",
                    e => { e.ConfigureConsumer<OrderCreatedEventConsumer>(context); });

                config.ReceiveEndpoint("queue-product-create",
                    e => { e.ConfigureConsumer<ProductCreatedEvetConsumer>(context); });

                // if excepttion throws  while the messages is  processing in consumer, mass transit  retry messages
                config.UseMessageRetry(retryConfigurator =>
                    {
                        // also can use
                        //  retryConfigurator.Immediate(5);
                        retryConfigurator.Incremental(
                            3,
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(15)
                        );
                    }
                );
                //the message is removed from the queue and then redelivered to the queue at a future time.
                //RabbitMQ required a delayed-exchange plug-in
                config.UseScheduledRedelivery(retryConfigurator =>
                    {
                        retryConfigurator.Intervals(
                            TimeSpan.FromMinutes(1),
                            TimeSpan.FromMinutes(2),
                            TimeSpan.FromMinutes(3)
                        );
                    }
                );

                config.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();