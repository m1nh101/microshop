using Common.EventBus;

namespace User.API.Domain.Events;

public sealed record UserRemovedEvent(
  Entities.User User) : IDomainEvent;
