using Redis.OM.Modeling;

namespace User.API.Infrastructure.Caching.Models;

[Document(Prefixes = ["Token"], StorageType = StorageType.Hash)]
public sealed class UserToken
{
    [Indexed]
    public required string UserId { get; init; }
    [Searchable]
    public required string RefreshToken { get; init; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}
