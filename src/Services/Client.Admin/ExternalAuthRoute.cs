using API.Contract.Users.Requests;
using Client.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace Client.Admin;

public static class ExternalAuthRoute
{
  public static void UseExternalAuthRoute(this IEndpointRouteBuilder router)
  {
    router.MapPost("/auth", Authenticate);
    router.MapPost("/register", Register);
  }

  static async Task Authenticate(
    HttpContext context,
    UserService service)
  {
    var request = new AuthenticateRequest(
      Username: context.Request.Form["username"]!,
      Password: context.Request.Form["password"]!);

    var response = await service.Authenticate(request);

    if(response.IsSuccess)
    {
      context.Response.Cookies.Append("access_token", response.Data!.AccessToken, new CookieOptions
      {
        SameSite = SameSiteMode.None,
        HttpOnly = true,
        Secure = true,
      });

      context.Response.Redirect("/");
    }
  }

  static async Task Register(
    [FromServices] UserService service,
    [FromServices] HttpContext context,
    [FromBody] RegisterRequest req)
  {
    var response = await service.Register(req);

    if (response.IsSuccess)
      context.Response.Redirect("/login");
  }
}
