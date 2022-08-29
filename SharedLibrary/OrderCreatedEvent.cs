namespace SharedLibrary;

public class OrderCreatedEvent
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public string Name { get; set; }
}