namespace User.API.Application.Contracts;

public interface IUserRepository
{
  Task<Infrastructure.Entities.User?> GetUser(string username);
}
