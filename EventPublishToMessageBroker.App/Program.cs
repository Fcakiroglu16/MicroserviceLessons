// See https://aka.ms/new-console-template for more information

using MassTransit;
using SharedEvents;

Console.WriteLine("Mesajlar Gönderiliyor");

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host("localhost", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });
});

foreach (var i in Enumerable.Range(1, 21).ToList())
{
    var userName = $"jone-{i}";

// Epoch time can use for this situation
    var userNameChangedEvent = new UserNameChangedEvent(1, userName,DateTime.Now.ToUniversalTime());

    await busControl.Publish(userNameChangedEvent);

    Console.WriteLine($"Mesaj gönderildi: {userNameChangedEvent}");
}