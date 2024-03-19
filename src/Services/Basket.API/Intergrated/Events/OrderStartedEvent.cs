using Common;
using FastEndpoints;

namespace Basket.API.Intergrated.Events;

public record OrderStartedEvent(
  string UserId) : IntergratedEvent, ICommand;
