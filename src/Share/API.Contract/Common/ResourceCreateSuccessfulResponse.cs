namespace API.Contract.Common;
public record ResourceCreateSuccessfulResponse
{
  public required string Id { get; init; }
  public required DateTime CreatedAt { get; init; }
}
