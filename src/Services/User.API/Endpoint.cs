using API.Contract.Users.Requests;
using API.Contract.Users.Responses;
using Common;
using Common.Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using User.API.Application.Handlers;

namespace User.API;

public static class Endpoint
{
  public static IEndpointRouteBuilder UseUserAPIEndpoint(this IEndpointRouteBuilder builder)
  {
    builder.MapPost("/api/users/auth", AuthenticateEndpoint);
    builder.MapPost("/api/users/register", RegisterEndpoint);
    builder.MapPost("/api/users/token", RequireAccessTokenEndpoint);
    builder.MapPost("/api/users/logout", SignoutEndpoint)
      .RequireAuthorization();

    return builder;
  }

  private static async Task<Ok<Result<AuthenticateResponse>>> AuthenticateEndpoint(
    [FromServices] IMediator mediator,
    HttpContext httpContext,
    [FromBody] AuthenticateRequest request)
  {
    var userAgent = httpContext.Request.Headers.UserAgent;
    var command = new AuthenticateCommand(request.Username, request.Password, userAgent!);
    var response = await mediator.Send(command)
      .As<Result<AuthenticateResponse>>();

    if(response.IsSuccess)
    {
      httpContext.Response.Cookies.Append("access_token", response.Data!.AccessToken, new CookieOptions
      {
        Secure = true,
        HttpOnly = true,
        SameSite = SameSiteMode.None,
        Path = "/"
      });

      httpContext.Response.Cookies.Append("refresh_token", response.Data!.RefreshToken, new CookieOptions
      {
        Secure = true,
        HttpOnly = true,
        SameSite = SameSiteMode.None,
        Path = "/token"
      });
    }

    return TypedResults.Ok(response);
  }

  private static async Task<Ok<Result<RegisterResponse>>> RegisterEndpoint(
    [FromServices] IMediator mediator,
    [FromBody] RegisterRequest request)
  {
    var result = await mediator.Send(request)
      .As<Result<RegisterResponse>>();

    return TypedResults.Ok(result);
  }

  private static async Task<Ok<Result<RequireAccessTokenResponse>>> RequireAccessTokenEndpoint(
    [FromServices] IMediator mediator,
    HttpContext httpContext)
  {
    var userAgent = httpContext.Request.Headers.UserAgent;
    var refreshToken = httpContext.Request.Cookies["refresh_token"]?.ToString()
      ?? httpContext.Request.Query["refreshToken"].ToString();
    var request = new RequireAccessTokenCommand(refreshToken, userAgent!);
    var response = await mediator.Send(request)
      .As<Result<RequireAccessTokenResponse>>();

    return TypedResults.Ok(response);
  }

  private static async Task<NoContent> SignoutEndpoint(
    [FromServices] IMediator mediator,
    HttpContext httpContext)
  {
    await mediator.Send(new RevokeTokenCommand())
      .As<Error>();

    httpContext.Response.Cookies.Delete("refresh_token");
    httpContext.Response.Cookies.Delete("access_token");

    return TypedResults.NoContent();
  }
}
