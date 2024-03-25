using Client.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace Client.Admin;

public static class ExternalAuthRoute
{
  public static void UseExternalAuthRoute(this IEndpointRouteBuilder router)
  {
    router.MapPost("/logout", SignOut);
  }

  static async Task SignOut(
    UserService service,
    HttpContext context)
  {
    await service.Signout();

    context.Response.Cookies.Delete("refresh_token");
    context.Response.Cookies.Delete("access_token");
    context.Response.Redirect("/login");
  }
}
