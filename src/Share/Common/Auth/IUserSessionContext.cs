using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Common.Auth;

public interface IUserSessionContext
{
  string UserId { get; }
  string Name { get; }
}

public class UserSessionContext : IUserSessionContext
{
  private readonly HttpContext _httpContext;

  public UserSessionContext(IHttpContextAccessor context)
  {
    _httpContext = context.HttpContext!;
  }

  string IUserSessionContext.UserId => _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
    ?? throw new UnauthorizedAccessException();

  string IUserSessionContext.Name => _httpContext.User.FindFirstValue(ClaimTypes.GivenName)
    ?? throw new UnauthorizedAccessException();
}
