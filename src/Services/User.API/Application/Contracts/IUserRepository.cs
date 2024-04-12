namespace User.API.Application.Contracts;

public interface IUserRepository
{
  Task<Domain.Entities.User?> GetUser(string username);
}
