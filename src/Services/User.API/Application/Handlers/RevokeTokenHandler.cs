using Common;
using Common.Auth;
using Common.Mediator;
using User.API.Infrastructure.Caching;

namespace User.API.Application.Handlers;

public record RevokeTokenCommand;

public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand>
{
  private readonly UserTokenCachingStorage _cache;
  private readonly IUserSessionContext _session;

  public RevokeTokenHandler(
    UserTokenCachingStorage cache,
    IUserSessionContext session)
  {
    _cache = cache;
    _session = session;
  }

  public async Task<Result> Handle(RevokeTokenCommand request)
  {
    await _cache.RevokeRefreshToken(_session.UserId);

    return Result.Ok();
  }
}
