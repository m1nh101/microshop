namespace Common.EventBus;

public interface IDomainEventHandler<TEvent>
  where TEvent : IDomainEvent
{
  Task Handle(TEvent @event);
}
