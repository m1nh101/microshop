using Common;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using User.API.Application.Contracts;
using User.API.Http.Requests;
using User.API.Http.Responses;
using User.API.Infrastructure.Caching;
using User.API.Infrastructure.Caching.Models;
using User.API.Infrastructure.Database;

namespace User.API.Http.Endpoints;

[AllowAnonymous]
[HttpPost("/api/users/auth")]
public sealed class AuthenticateEndpoint : Endpoint<AuthenticateRequest, Result<AuthenticateResponse>>
{
  private readonly UserDbContext _context;
  private readonly UserTokenCachingService _cache;
  private readonly IPasswordGenerator _passwordGenerator;
  private readonly IAccessTokenGenerator _accessTokenGenerator;
  private readonly IRefreshTokenGenerator _refreshTokenGenerator;


  public AuthenticateEndpoint(
    UserDbContext context,
    IPasswordGenerator passwordGenerator,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    UserTokenCachingService cache)
  {
    _context = context;
    _passwordGenerator = passwordGenerator;
    _accessTokenGenerator = accessTokenGenerator;
    _refreshTokenGenerator = refreshTokenGenerator;
    _cache = cache;
  }

  public override async Task HandleAsync(AuthenticateRequest req, CancellationToken ct)
  {
    // check user credential info
    var user = await _context.Users
      .Include(e => e.Roles)
      .FirstOrDefaultAsync(e => e.Username == req.Username, ct);

    if (user == null)
    {
      var error = Result<AuthenticateResponse>.Failed(new Error("User.NotFound", "username/email is not exist in system"));
      await SendAsync(
        response: error,
        statusCode: 400,
        cancellation: ct);
      return;
    }

    var password = _passwordGenerator.Generate(req.Password);
    if(password != user.Password)
    {
      var error = Result<AuthenticateResponse>.Failed(new Error("User.WrongPassword", "password is incorrect!"));
      await SendAsync(
        response: error,
        statusCode: 400,
        cancellation: ct);
      return;
    }

    // get role of user;
    // maybe violet S in SOLID;
    var userRoles = user.Roles.Select(e => e.RoleId);
    var roles = await _context.Roles
      .AsNoTracking()
      .Where(e => userRoles.Any(d => d == e.Id))
      .Select(e => e.Name)
      .ToListAsync(ct);

    // generate access token
    var accessToken = _accessTokenGenerator.Generate(user, roles);
    var userToken = new UserToken()
    {
      UserId = user.Id,
      RefreshToken = _refreshTokenGenerator.Generate()
    };

    // store access token to cache system
    await _cache.AddUserToken(userToken);
    
    await SendAsync(
      response: Result<AuthenticateResponse>.Ok(new(user.Id, accessToken, userToken.RefreshToken)),
      statusCode: 200,
      cancellation: ct);
  }
}
