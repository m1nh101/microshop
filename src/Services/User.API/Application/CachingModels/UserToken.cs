using Redis.OM.Modeling;

namespace User.API.Application.CachingModels;

[Document(Prefixes = ["Token"], StorageType = StorageType.Hash)]
public sealed class UserToken
{
  [Indexed]
  public required string UserId { get; init; }
  [Searchable]
  public required string RefreshToken { get; init; }
  [Searchable]
  public required string UserAgent { get; init; }
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}
