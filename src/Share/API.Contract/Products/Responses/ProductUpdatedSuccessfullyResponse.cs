using API.Contract.Common;

namespace API.Contract.Products.Responses;

public record ProductUpdatedSuccessfullyResponse : ResourceUpdateSuccessfulResponse
{
  public required IEnumerable<object> UnitUpdateErrors { get; init; }
}
