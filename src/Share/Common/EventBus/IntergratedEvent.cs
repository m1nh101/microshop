namespace Common.EventBus;

public record IntergratedEvent
{
  public IntergratedEvent()
  {
    CreatedAt = DateTime.Now;
  }

  public DateTime CreatedAt { get; }
}
