namespace Common.EventBus;

public interface IDomainEvent
{
}


public abstract class DomainEvent
{
  public Queue<IDomainEvent> Events { get; set; } = new Queue<IDomainEvent>();

  public void AddEvent(IDomainEvent @event) => Events.Enqueue(@event);
  public void RemoveEvent() => Events.Dequeue();
}
