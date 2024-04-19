namespace API.Contract.Common;

public record ResourceUpdateSuccessfulResponse
{
  public required string Id { get; init; }
  public required DateTime ModifiedAt { get; init; }
}
