using Redis.OM.Contracts;
using Redis.OM.Searching;
using User.API.Application.CachingModels;

namespace User.API.Infrastructure.Caching;

public sealed class UserCredentialCachingStorage
{
  private readonly IRedisCollection<UserCredential> _credentials;

  public UserCredentialCachingStorage(IRedisConnectionProvider provider)
  {
    _credentials = provider.RedisCollection<UserCredential>();
  }

  public async Task CachingNewCredential(UserCredential credential)
  {
    await _credentials.InsertAsync(credential);
  }

  public async Task<Domain.Entities.User?> GetUserCredential(string username)
  {
    var cacheCredential = await _credentials.FirstOrDefaultAsync(e => e.Username == username || e.Email ==  username);
    if (cacheCredential is null)
      return null;

    var user = new Domain.Entities.User(
      username: username,
      name: string.Empty,
      email: username,
      phone: string.Empty,
      password: cacheCredential.Password);
    
    foreach(var role in cacheCredential.Roles)
      user.AddToRole(role);

    return user;
  }
}
