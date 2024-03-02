using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using User.API.Application.Contracts;

namespace User.API.Application.Helpers;

public sealed class PasswordGenerator : IPasswordGenerator
{
  private readonly IConfiguration _configuration;

  public PasswordGenerator(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public string Generate(string plainText)
  {
    var salt = _configuration["PASSWORD_SALT"] ?? throw new NullReferenceException();
    var hashPassword = new StringBuilder();
    var source = Encoding.UTF8.GetBytes(plainText + salt);
    var hash = MD5.HashData(source);

    foreach (var character in hash)
      hashPassword.Append(character.ToString("x2"));

    return hashPassword.ToString();
  }
}
