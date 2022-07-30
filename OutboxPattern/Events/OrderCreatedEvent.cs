namespace OutboxPattern.Events;

public record OrderCreatedEvent
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public int Count { get; init; }
}