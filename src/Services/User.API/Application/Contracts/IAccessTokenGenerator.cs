namespace User.API.Application.Contracts;

public interface IAccessTokenGenerator
{
  string Generate(Infrastructure.Entities.User user, IEnumerable<string> roles);
}
