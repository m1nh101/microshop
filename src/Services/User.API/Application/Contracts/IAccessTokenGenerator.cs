namespace User.API.Application.Contracts;

public interface IAccessTokenGenerator
{
  string Generate(Domain.Entities.User user, IEnumerable<string> roles);
}
