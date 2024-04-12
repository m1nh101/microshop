using Common.EventBus;
using User.API.Application.Contracts;
using User.API.Domain.Events;

namespace User.API.Application.EventHandlers;

public sealed class UserConfirmedEventHandler : IDomainEventHandler<UserConfirmedEvent>
{
  private readonly IUserConfirmationStorage _storage;

  public UserConfirmedEventHandler(IUserConfirmationStorage storage)
  {
    _storage = storage;
  }

  public async Task Handle(UserConfirmedEvent @event)
  {
    await _storage.Remove(@event.UserId);
  }
}
