using Redis.OM.Modeling;

namespace User.API.Infrastructure.Caching.Models;

[Document(Prefixes = ["Credential"], StorageType = StorageType.Hash)]
public sealed class UserCredential
{
  [Indexed, RedisIdField]
  public string Username { get; set; } = string.Empty;
  [Indexed]
  public string Email { get; set; } = string.Empty;
  [Searchable]
  public string Password { get; set; } = string.Empty;

  [RedisField]
  public string[] Roles { get; set; } = [];
}