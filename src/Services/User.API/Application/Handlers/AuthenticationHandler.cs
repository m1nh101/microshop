using API.Contract.Users.Requests;
using API.Contract.Users.Responses;
using Common;
using Common.Mediator;
using User.API.Application.Contracts;
using User.API.Infrastructure.Caching;
using User.API.Infrastructure.Caching.Models;

namespace User.API.Application.Handlers;

public record AuthenticateCommand(
  string Username,
  string Password,
  string UserAgent) : AuthenticateRequest(Username, Password);

public class AuthenticationHandler : IRequestHandler<AuthenticateCommand>
{
  private readonly IUserRepository _userRepository;
  private readonly UserTokenCachingService _cache;
  private readonly IPasswordGenerator _passwordGenerator;
  private readonly IAccessTokenGenerator _accessTokenGenerator;
  private readonly IRefreshTokenGenerator _refreshTokenGenerator;

  public AuthenticationHandler(
    IUserRepository userRepository,
    UserTokenCachingService cache,
    IPasswordGenerator passwordGenerator,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator)
  {
    _cache = cache;
    _userRepository = userRepository;
    _passwordGenerator = passwordGenerator;
    _accessTokenGenerator = accessTokenGenerator;
    _refreshTokenGenerator = refreshTokenGenerator;
  }

  public async Task<Result> Handle(AuthenticateCommand request)
  {
    var user = await _userRepository.GetUser(request.Username);

    if (user == null)
      return Result.Failed(Errors.InvalidUsernameEmail);

    var password = _passwordGenerator.Generate(request.Password);
    if (password != user.Password)
      return Result.Failed(Errors.WrongPassword);

    // generate access token
    var accessToken = _accessTokenGenerator.Generate(user, user.Roles.Select(d => d.Name));
    var refreshToken = _refreshTokenGenerator.Generate();
    var userToken = new UserToken()
    {
      UserId = user.Id,
      RefreshToken = refreshToken,
      UserAgent = request.UserAgent
    };

    // store access token to cache system
    await _cache.AddUserToken(userToken);

    return Result.Ok<AuthenticateResponse>(new(user.Id, accessToken, userToken.RefreshToken));
  }
}
