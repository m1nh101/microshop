using Auth;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using User.API.Infrastructure.Caching;

namespace User.API.Endpoints;

[Authorize]
[HttpDelete("/api/users/sign-out")]
public sealed class SignoutEndpoint : Endpoint<EmptyRequest, EmptyResponse>
{
  private readonly UserTokenCachingService _cache;
  private readonly IUserSessionContext _session;

  public SignoutEndpoint(
    UserTokenCachingService cache,
    IUserSessionContext session)
  {
    _cache = cache;
    _session = session;
  }

  public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
  {
    await _cache.RevokeRefreshToken(_session.UserId);

    HttpContext.Response.Cookies.Delete("access_token");
    HttpContext.Response.Cookies.Delete("refresh_token");

    await SendNoContentAsync(ct);
  }
}
