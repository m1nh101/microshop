namespace Common.EventBus;

public record IntergratedEvent
{
  public IntergratedEvent()
  {
    CreatedAt = DateTime.Now;
  }

  public DateTime CreatedAt { get; }
}

public interface IEventHandler
{
  Task Handle(IntergratedEvent @event);
}

public interface IEventHandler<in TEvent>
  : IEventHandler
  where TEvent : IntergratedEvent
{
  Task Handle(TEvent @event);
  Task IEventHandler.Handle(IntergratedEvent @event) => Handle((TEvent)@event);
}
