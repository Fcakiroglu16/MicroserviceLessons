using MassTransit;

namespace Microservice1.API;

public class EventConsumer : IConsumer<Event>
{
    public Task Consume(ConsumeContext<Event> context)
    {
        Console.WriteLine("Retry Consumer");
        //throw new DivideByZeroException("error message");

        Console.WriteLine($"Event has been processed: Event ID :{context.Message.Id}");
        return Task.CompletedTask;
    }
}