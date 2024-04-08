namespace Common.EventBus;

public interface IEventBus
{
  Task Publish<TMessage>(TMessage message)
    where TMessage : IntergratedEvent;
}
