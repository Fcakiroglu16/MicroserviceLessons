namespace SharedLibrary;

public abstract record BaseEvent
{
    public Guid IdempotentToken = Guid.NewGuid();
    public Guid EventId => Guid.NewGuid();
}