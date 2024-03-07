using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth;

public static class JwtParser
{
  public static ClaimsIdentity Decode(string jwtToken)
  {
    var handler = new JwtSecurityTokenHandler();
    var securityToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

    return securityToken is null ? throw new NullReferenceException() : new ClaimsIdentity(securityToken.Claims);
  }
}
