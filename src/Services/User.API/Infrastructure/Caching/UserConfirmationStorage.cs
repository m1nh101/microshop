using Redis.OM.Contracts;
using Redis.OM.Searching;
using User.API.Application.CachingModels;
using User.API.Application.Contracts;

namespace User.API.Infrastructure.Caching;

public class UserConfirmationStorage : IUserConfirmationStorage
{
  private readonly IRedisCollection<UserConfirmation> _storage;

  public UserConfirmationStorage(IRedisConnectionProvider provider)
  {
    _storage = provider.RedisCollection<UserConfirmation>();
  }

  public Task Add(UserConfirmation userConfirmation) => _storage.InsertAsync(userConfirmation, TimeSpan.FromHours(24));
  public async Task Remove(string userId)
  {
    var confirmation = await _storage.FindByIdAsync(userId);
    if (confirmation != null)
      await _storage.DeleteAsync(confirmation);
  }

  public async Task<bool> ConfirmUser(string id, string confirmationCode)
  {
    var confirmation = await _storage.FindByIdAsync(id) ?? throw new NullReferenceException();

    return confirmation.ConfirmationCode == confirmationCode;
  }

  public async Task<string?> ConfirmUser(string token)
  {
    var confirmation = await _storage.FirstOrDefaultAsync(e => e.Token == token);
    if (confirmation is null)
      return null;

    return confirmation.Id;
  }

  public Task<bool> IsExistConfirmationCode(string confirmationCode) => _storage.AnyAsync(e => e.ConfirmationCode == confirmationCode);
}
