using Common.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using User.API.Application.Contracts;

namespace User.API.Application.Helpers;

public sealed class TokenGenerator : IRefreshTokenGenerator, IAccessTokenGenerator
{
  private readonly TokenOption _tokenOption;

  public TokenGenerator(IOptions<TokenOption> option)
  {
    _tokenOption = option.Value;
  }

  string IAccessTokenGenerator.Generate(Infrastructure.Entities.User user, IEnumerable<string> roles)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(_tokenOption.SecretKey);
    var roleClaims = roles
      .Select(e => new Claim(ClaimTypes.Role, e));

    var claims = new List<Claim>()
    {
      new(ClaimTypes.NameIdentifier, user.Id),
      new(ClaimTypes.Name, user.Username)
    }.Union(roleClaims);

    var tokenDescription = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddSeconds(_tokenOption.ExpiredIn),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
    };

    var token = tokenHandler.CreateToken(tokenDescription);

    return tokenHandler.WriteToken(token);
  }

  string IRefreshTokenGenerator.Generate()
  {
    var randomNumber = new byte[64];
    var dateTimeString = DateTime.UtcNow.ToLongDateString();
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    var bytes = randomNumber.Concat(Encoding.UTF8.GetBytes(dateTimeString));
    return Convert.ToBase64String(randomNumber);
  }
}
