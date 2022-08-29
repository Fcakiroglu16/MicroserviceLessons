namespace SharedLibrary;

public record ProductCreatedEvent
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
}