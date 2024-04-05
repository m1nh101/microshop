using Auth;
using Common;
using Common.Mediator;
using User.API.Infrastructure.Caching;

namespace User.API.Application.Handlers;

public record RevokeTokenCommand;

public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand>
{
  private readonly UserTokenCachingService _cache;
  private readonly IUserSessionContext _session;

  public RevokeTokenHandler(
    UserTokenCachingService cache,
    IUserSessionContext session)
  {
    _cache = cache;
    _session = session;
  }

  public async Task<object> Handle(RevokeTokenCommand request)
  {
    await _cache.RevokeRefreshToken(_session.UserId);

    return Error.None;
  }
}
