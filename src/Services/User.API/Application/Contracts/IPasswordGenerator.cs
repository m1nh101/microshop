namespace User.API.Application.Contracts;

public interface IPasswordGenerator
{
  string Generate(string plainText);
}
