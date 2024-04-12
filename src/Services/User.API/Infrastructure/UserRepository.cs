using Microsoft.EntityFrameworkCore;
using User.API.Application.Contracts;
using User.API.Infrastructure.Caching;
using User.API.Infrastructure.Database;

namespace User.API.Infrastructure;

public class UserRepository : IUserRepository
{
  private readonly UserCredentialCachingService _userCredentialCachingService;
  private readonly UserDbContext _context;

  public UserRepository(
    UserCredentialCachingService userCredentialCachingService,
    UserDbContext context)
  {
    _userCredentialCachingService = userCredentialCachingService;
    _context = context;
  }

  public async Task<Entities.User?> GetUser(string username)
  {
    var cachedUser = await _userCredentialCachingService.GetUserCredential(username);

    if(cachedUser == null)
    {
      var user = await _context.Users
        .AsNoTracking()
        .Include(e => e.Roles)
        .FirstOrDefaultAsync(e => e.Username == username || e.Email == username);
      if (user is null)
        return null;

      await _userCredentialCachingService.CachingNewCredential(new Caching.Models.UserCredential
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
