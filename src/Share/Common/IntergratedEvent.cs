namespace Common;

public record IntergratedEvent
{
  public IntergratedEvent()
  {
    CreatedAt = DateTime.Now;
  }

  public DateTime CreatedAt { get; }
}
