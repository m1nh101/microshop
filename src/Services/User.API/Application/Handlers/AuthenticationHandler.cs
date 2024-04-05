using API.Contract.Users.Requests;
using API.Contract.Users.Responses;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using User.API.Application.Contracts;
using User.API.Infrastructure.Caching;
using User.API.Infrastructure.Caching.Models;
using User.API.Infrastructure.Database;

namespace User.API.Application.Handlers;

public record AuthenticateCommand(
  string Username,
  string Password,
  string UserAgent) : AuthenticateRequest(Username, Password);

public class AuthenticationHandler : IRequestHandler<AuthenticateCommand>
{
  private readonly UserDbContext _context;
  private readonly UserTokenCachingService _cache;
  private readonly IPasswordGenerator _passwordGenerator;
  private readonly IAccessTokenGenerator _accessTokenGenerator;
  private readonly IRefreshTokenGenerator _refreshTokenGenerator;

  public AuthenticationHandler(
    UserDbContext context,
    UserTokenCachingService cache,
    IPasswordGenerator passwordGenerator,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator)
  {
    _context = context;
    _cache = cache;
    _passwordGenerator = passwordGenerator;
    _accessTokenGenerator = accessTokenGenerator;
    _refreshTokenGenerator = refreshTokenGenerator;
  }

  public async Task<object> Handle(AuthenticateCommand request)
  {
    var user = await _context.Users
      .Include(e => e.Roles)
      .FirstOrDefaultAsync(e => e.Username == request.Username);

    if (user == null)
      return Result<AuthenticateResponse>.Failed(new Error("User.NotFound", "username/email is not exist in system"));


    var password = _passwordGenerator.Generate(request.Password);
    if (password != user.Password)
      return Result<AuthenticateResponse>.Failed(new Error("User.WrongPassword", "password is incorrect!"));

    // get role of user;
    // maybe violet S in SOLID;
    var userRoles = user.Roles.Select(e => e.RoleId);
    var roles = await _context.Roles
      .AsNoTracking()
      .Where(e => userRoles.Any(d => d == e.Id))
      .Select(e => e.Name)
      .ToListAsync();

    // generate access token
    var accessToken = _accessTokenGenerator.Generate(user, roles);
    var refreshToken = _refreshTokenGenerator.Generate();
    var userToken = new UserToken()
    {
      UserId = user.Id,
      RefreshToken = refreshToken,
      UserAgent = request.UserAgent
    };

    // store access token to cache system
    await _cache.AddUserToken(userToken);

    // write http-cookie to response

    return Result<AuthenticateResponse>.Ok(new(user.Id, accessToken, userToken.RefreshToken));
  }
}
