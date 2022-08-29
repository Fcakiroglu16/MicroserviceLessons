namespace WorkerServiceWithSubscriber.Models;

public class QueueIdempotentInbox
{
    public int Id { get; set; }
    public string Consumer { get; set; }
    public Guid MessageId { get; set; }
    public DateTime Created { get; set; }
}