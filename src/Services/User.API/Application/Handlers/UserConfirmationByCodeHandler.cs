using Common;
using Common.Auth;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using User.API.Application.Contracts;
using User.API.Infrastructure.Database;

namespace User.API.Application.Handlers;

public sealed record UserConfirmationByCodeRequest(
  string ConfirmationCode);

public class UserConfirmationByCodeHandler : IRequestHandler<UserConfirmationByCodeRequest>
{
  private readonly UserDbContext _context;
  private readonly IUserConfirmationStorage _storage;
  private readonly IUserSessionContext _session;

  public UserConfirmationByCodeHandler(
    UserDbContext context,
    IUserConfirmationStorage storage,
    IUserSessionContext session)
  {
    _context = context;
    _storage = storage;
    _session = session;
  }

  public async Task<Result> Handle(UserConfirmationByCodeRequest request)
  {
    var confirmationResult = await _storage.ConfirmUser(_session.UserId, request.ConfirmationCode);

    if (confirmationResult)
    {
      var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == _session.UserId);

      user!.Confirm();

      await _context.SaveChangesAsync();
    }

    return Result.Failed(Summary.InvalidPayload, Errors.InvalidConfirmationCode);
  }
}
