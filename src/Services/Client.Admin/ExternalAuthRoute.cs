namespace Client.Admin;

public static class ExternalAuthRoute
{
  public static void UseExternalAuthRoute(this IEndpointRouteBuilder router)
  {
    router.MapPost("/logout", SignOut).RequireAuthorization();
  }

  static async Task SignOut(
    RequestBuilder requestBuilder,
    HttpContext context)
  {
    var token = context.Request.Headers.Authorization.ToString()
      ?? context.Request.Cookies["access_token"]?.ToString()
      ?? string.Empty;

    await requestBuilder.WithEndpoint(Endpoints.User.Logout)
      .WithAuthorization(token)
      .WithHttpMethod(HttpMethod.DELETE)
      .Send();

    context.Response.Cookies.Delete("refresh_token");
    context.Response.Cookies.Delete("access_token");
    context.Response.Redirect("/login");
  }
}
