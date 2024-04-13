using Common;
using Common.Auth;
using Common.Mediator;
using User.API.Application.Contracts;

namespace User.API.Application.Handlers;

public record RevokeTokenCommand;

public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand>
{
  private readonly IUserTokenStorage _tokenStorage;
  private readonly IUserSessionContext _session;

  public RevokeTokenHandler(
    IUserTokenStorage tokenStorage,
    IUserSessionContext session)
  {
    _tokenStorage = tokenStorage;
    _session = session;
  }

  public async Task<Result> Handle(RevokeTokenCommand request)
  {
    await _tokenStorage.Remove(_session.UserId);

    return Result.Ok();
  }
}
