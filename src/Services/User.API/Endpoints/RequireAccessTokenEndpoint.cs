using API.Contract.Users.Requests;
using API.Contract.Users.Responses;
using Common;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using User.API.Application.Contracts;
using User.API.Infrastructure.Caching;
using User.API.Infrastructure.Database;

namespace User.API.Endpoints;

[AllowAnonymous]
[HttpGet("/api/users/token")]
public sealed class RequireAccessTokenEndpoint : Endpoint<RequireAccessTokenRequest, Result<RequireAccessTokenResponse>>
{
  private readonly UserTokenCachingService _cache;
  private readonly UserDbContext _context;
  private readonly IAccessTokenGenerator _accessTokenGenerator;

  public RequireAccessTokenEndpoint(
    UserTokenCachingService cache,
    IAccessTokenGenerator accessTokenGenerator,
    UserDbContext context)
  {
    _cache = cache;
    _accessTokenGenerator = accessTokenGenerator;
    _context = context;
  }

  public override async Task HandleAsync(RequireAccessTokenRequest req, CancellationToken ct)
  {
    // check token validation
    var token = await _cache.GetTokenByRefreshToken(req.RefreshToken);
    if (token == null)
    {
      await SendAsync(
        response: Errors.RefreshTokenIsNotValid,
        statusCode: 401,
        cancellation: ct);
      return;
    }

    var user = await _context.Users
      .Include(e => e.Roles)
      .FirstOrDefaultAsync(e => e.Id == token.UserId, ct);
    var userRoles = user!.Roles.Select(e => e.RoleId);
    var roles = await _context.Roles
      .AsNoTracking()
      .Where(e => userRoles.Any(d => d == e.Id))
      .Select(e => e.Name)
      .ToListAsync(ct);

    var accessToken = _accessTokenGenerator.Generate(user, roles);

    await SendAsync(
      response: new RequireAccessTokenResponse(accessToken),
      statusCode: 200,
      cancellation: ct);
  }
}
