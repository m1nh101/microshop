using API.Contract.Users.Requests;
using API.Contract.Users.Responses;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using User.API.Application.Contracts;
using User.API.Infrastructure.Caching;
using User.API.Infrastructure.Database;

namespace User.API.Application.Handlers;

public record RequireAccessTokenCommand(
  string RefreshToken,
  string UserAgent) : RequireAccessTokenRequest(RefreshToken);

public class RequireAccessTokenHandler : IRequestHandler<RequireAccessTokenCommand>
{
  private readonly UserTokenCachingService _cache;
  private readonly UserDbContext _context;
  private readonly IAccessTokenGenerator _accessTokenGenerator;

  public RequireAccessTokenHandler(
    UserTokenCachingService cache,
    UserDbContext context,
    IAccessTokenGenerator accessTokenGenerator)
  {
    _cache = cache;
    _context = context;
    _accessTokenGenerator = accessTokenGenerator;
  }

  public async Task<Result> Handle(RequireAccessTokenCommand request)
  {
    // check token validation
    var token = await _cache.GetTokenByRefreshToken(request.RefreshToken, request.UserAgent);
    if (token == null)
      return Result.Failed(Errors.RefreshTokenIsNotValid);

    var user = await _context.Users
      .Include(e => e.Roles)
      .FirstOrDefaultAsync(e => e.Id == token.UserId);
    var userRoles = user!.Roles.Select(e => e.RoleId);
    var roles = await _context.Roles
      .AsNoTracking()
      .Where(e => userRoles.Any(d => d == e.Id))
      .Select(e => e.Name)
      .ToListAsync();

    var accessToken = _accessTokenGenerator.Generate(user, roles);

    return Result.Ok(new RequireAccessTokenResponse(accessToken));
  }
}
