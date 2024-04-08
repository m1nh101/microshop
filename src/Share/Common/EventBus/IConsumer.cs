namespace Common.EventBus;

public interface IConsumer<TMessage>
  where TMessage : IntergratedEvent
{
  Task Consume(TMessage message);
}