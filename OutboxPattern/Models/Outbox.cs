namespace OutboxPattern.Models;

public class Outbox
{
    public int Id { get; set; }

    public string Payload { get; set; } 

    public bool IsSendBus { get; set; }
    public DateTime Created { get; set; }
}