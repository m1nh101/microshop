using API.Contract.Users.Requests;
using API.Contract.Users.Responses;
using Common;
using Common.Auth;
using Common.Mediator;
using User.API.Application.CachingModels;
using User.API.Application.Contracts;

namespace User.API.Application.Handlers;

public record AuthenticateCommand(
  string Username,
  string Password,
  string UserAgent) : AuthenticateRequest(Username, Password);

public class AuthenticationHandler : IRequestHandler<AuthenticateCommand>
{
  private readonly IUserRepository _userRepository;
  private readonly IUserTokenStorage _tokenStorage;
  private readonly IPasswordGenerator _passwordGenerator;
  private readonly IAccessTokenGenerator _accessTokenGenerator;
  private readonly IRefreshTokenGenerator _refreshTokenGenerator;

  public AuthenticationHandler(
    IUserRepository userRepository,
    IUserTokenStorage tokenStorage,
    IPasswordGenerator passwordGenerator,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator)
  {
    _tokenStorage = tokenStorage;
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
    await _tokenStorage.Add(userToken);

    var isAdmin = user.Roles.Any(e => e.Name == PolicyName.Admin);

    return Result.Ok<AuthenticateResponse>(new(user.Id, accessToken, userToken.RefreshToken, isAdmin));
  }
}
