using Microsoft.EntityFrameworkCore;
using User.API.Application.CachingModels;
using User.API.Application.Contracts;
using User.API.Infrastructure.Caching;

namespace User.API.Infrastructure.Database.Repositories;

public class UserRepository : IUserRepository
{
  private readonly UserCredentialCachingStorage _userCredentialCachingService;
  private readonly UserDbContext _context;

  public UserRepository(
    UserCredentialCachingStorage userCredentialCachingService,
    UserDbContext context)
  {
    _userCredentialCachingService = userCredentialCachingService;
    _context = context;
  }

  public async Task<Domain.Entities.User?> GetUser(string username)
  {
    var cachedUser = await _userCredentialCachingService.GetUserCredential(username);

    if (cachedUser == null)
    {
      var user = await _context.Users
        .AsNoTracking()
        .Include(e => e.Roles)
        .FirstOrDefaultAsync(e => e.Username == username || e.Email == username);
      if (user is null)
        return null;

      await _userCredentialCachingService.CachingNewCredential(new UserCredential
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
