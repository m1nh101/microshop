﻿using Redis.OM.Contracts;
using Redis.OM.Searching;
using User.API.Infrastructure.Caching.Models;

namespace User.API.Infrastructure.Caching;

public sealed class UserTokenCachingService
{
  private readonly IRedisCollection<UserToken> _tokens;

  public UserTokenCachingService(IRedisConnectionProvider provider)
  {
    _tokens = provider.RedisCollection<UserToken>();
  }

  public async Task AddUserToken(UserToken token) => await _tokens.InsertAsync(token, TimeSpan.FromDays(7));
  public async Task<UserToken?> GetTokenByRefreshToken(string refreshToken)
  {
    return await _tokens.FirstOrDefaultAsync(e => e.RefreshToken == refreshToken);
  }
}
