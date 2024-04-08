namespace Common.EventBus;

public interface IProducer<TMessage>
  where TMessage : IntergratedEvent
{
  Task Publish(TMessage message);
}