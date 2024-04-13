using User.API.Application.CachingModels;

namespace User.API.Application.Contracts;

public interface IUserTokenStorage
{
  Task Add(UserToken token);
  Task Remove(string userId);
}