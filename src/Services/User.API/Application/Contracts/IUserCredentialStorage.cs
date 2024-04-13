using User.API.Application.CachingModels;

namespace User.API.Application.Contracts;

public interface IUserCredentialStorage
{
  Task Add(UserCredential credential);
  Task<Domain.Entities.User?> GetUserCredential(string username);
}
