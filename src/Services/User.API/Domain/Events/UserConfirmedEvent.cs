using Common.EventBus;

namespace User.API.Domain.Events;

public sealed record UserConfirmedEvent(
  string UserId) : IDomainEvent;
