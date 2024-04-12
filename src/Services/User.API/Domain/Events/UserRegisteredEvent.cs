using Common.EventBus;

namespace User.API.Domain.Events;

public sealed record UserRegisteredEvent(
  Entities.User User) : IDomainEvent;
