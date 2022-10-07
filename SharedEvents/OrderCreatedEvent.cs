namespace SharedEvents;

public class OrderCreatedEvent
{
// this property imply that order sequence ( example : 1.order, 2.order,3.order
    public int OrderSequence { get; set; }
    public int OrderId { get; set; }
    public int Count { get; set; }

    public override string ToString()
    {
        return $"Order Id :{OrderId}, Order Count :{Count}";
    }
}