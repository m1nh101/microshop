using User.API.Application.CachingModels;

namespace User.API.Application.Contracts;

public interface IUserConfirmationStorage
{
  Task Add(UserConfirmation userConfirmation);
  Task Remove(string userId);
  Task<bool> ConfirmUser(string id, string confirmationCode);
  Task<string?> ConfirmUser(string token);
  Task<bool> IsExistConfirmationCode(string confirmationCode);
}
