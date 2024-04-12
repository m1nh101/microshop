using Redis.OM.Contracts;
using Redis.OM.Searching;
using User.API.Application.CachingModels;

namespace User.API.Infrastructure.Caching;

public sealed class UserTokenCachingStorage
{
  private readonly IRedisCollection<UserToken> _tokens;

  public UserTokenCachingStorage(IRedisConnectionProvider provider)
  {
    _tokens = provider.RedisCollection<UserToken>();
  }

  public async Task AddUserToken(UserToken token) => await _tokens.InsertAsync(token, TimeSpan.FromDays(7));
  public async Task<UserToken?> GetTokenByRefreshToken(string refreshToken, string userAgent)
  {
    return await _tokens.FirstOrDefaultAsync(e => e.RefreshToken == refreshToken && e.UserAgent == userAgent);
  }

  public async Task RevokeRefreshToken(string userId)
  {
    var token = await _tokens.FirstOrDefaultAsync(e => e.UserId == userId);
    if (token is null) return;
    await _tokens.DeleteAsync(token);
  }
}
