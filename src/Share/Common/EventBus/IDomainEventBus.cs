namespace Common.EventBus;

public interface IDomainEventBus
{
  Task Publish<TEvent>(TEvent @event) where TEvent : IDomainEvent;
}
