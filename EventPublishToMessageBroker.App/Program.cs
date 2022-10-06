// See https://aka.ms/new-console-template for more information

using MassTransit;
using SharedEvents;


Console.WriteLine("Mesajlar Gönderiliyor");

Console.WriteLine("Messages publishing");
var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host("localhost", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });
});

foreach (var i in Enumerable.Range(1, 50).ToList())
{
    var orderCreatedEvent = new OrderCreatedEvent { OrderId = 1, Count = 1  ,OrderSequence = i};

    await busControl.Publish(orderCreatedEvent);

    Console.WriteLine($"Mesaj gönderildi: {orderCreatedEvent}");
}