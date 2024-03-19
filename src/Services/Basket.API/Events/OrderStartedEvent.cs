using Common;
using FastEndpoints;

namespace Basket.API.Events;

public record OrderStartedEvent(
  string UserId) : IntergratedEvent, ICommand;
