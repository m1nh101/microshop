using Microsoft.EntityFrameworkCore;
using User.API.Application.CachingModels;
using User.API.Application.Contracts;

namespace User.API.Infrastructure.Database.Repositories;

public class UserRepository : IUserRepository
{
  private readonly IUserCredentialStorage _credentialStorage;
  private readonly UserDbContext _context;

  public UserRepository(
    IUserCredentialStorage credentialStorage,
    UserDbContext context)
  {
    _credentialStorage = credentialStorage;
    _context = context;
  }

  public async Task<Domain.Entities.User?> GetUser(string username)
  {
    var cachedUser = await _credentialStorage.GetUserCredential(username);

    if (cachedUser == null)
    {
      var user = await _context.Users
        .AsNoTracking()
        .Include(e => e.Roles)
        .FirstOrDefaultAsync(e => e.Username == username || e.Email == username);
      if (user is null)
        return null;

      await _credentialStorage.Add(new UserCredential
      {
        Email = user.Email,
        Password = user.Password,
        Username = user.Username,
        Roles = user.Roles.Select(e => e.Name).ToArray()
      });
      return user;
    }

    return cachedUser;
  }
}
