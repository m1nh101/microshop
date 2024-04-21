using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using User.API.Application.Contracts;
using User.API.Infrastructure.Database;

namespace User.API.Application.Handlers;

public sealed record UserConfirmationByUrlRequest(
  string Token);

public class UserConfirmationByUrlHandler : IRequestHandler<UserConfirmationByUrlRequest>
{
  private readonly UserDbContext _context;
  private readonly IUserConfirmationStorage _storage;

  public UserConfirmationByUrlHandler(
    UserDbContext context,
    IUserConfirmationStorage storage)
  {
    _context = context;
    _storage = storage;
  }

  public async Task<Result> Handle(UserConfirmationByUrlRequest request)
  {
    var userId = await _storage.ConfirmUser(request.Token);
    if (string.IsNullOrEmpty(userId))
      return Result.Failed(Summary.InvalidPayload, Errors.InvalidConfirmationCode);

    var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == userId);

    user!.Confirm();

    await _context.SaveChangesAsync();

    return Result.Ok();
  }
}
