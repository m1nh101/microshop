using API.Contract.Users.Requests;
using API.Contract.Users.Responses;
using Common;
using Common.Mediator;
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

  private static async Task<IResult> AuthenticateEndpoint(
    [FromServices] IMediator mediator,
    HttpContext httpContext,
    [FromBody] AuthenticateRequest request)
  {
    var userAgent = httpContext.Request.Headers.UserAgent;
    var command = new AuthenticateCommand(request.Username, request.Password, userAgent!);
    var result = await mediator.Send(command);

    if (result.IsSuccess)
    {
      var data = result.As<AuthenticateResponse>();

      httpContext.Response.Cookies.Append("access_token", data.AccessToken, new CookieOptions
      {
        Secure = true,
        HttpOnly = true,
        SameSite = SameSiteMode.None,
        Path = "/"
      });

      httpContext.Response.Cookies.Append("refresh_token", data.RefreshToken, new CookieOptions
      {
        Secure = true,
        HttpOnly = true,
        SameSite = SameSiteMode.None,
        Path = "/token"
      });

      return TypedResults.Ok(result.Data);
    }

    return TypedResults.BadRequest(result.Error);
  }

  private static async Task<IResult> RegisterEndpoint(
    [FromServices] IMediator mediator,
    [FromBody] RegisterRequest request)
  {
    var result = await mediator.Send(request);

    return GenerateHttpResponse(result);
  }

  private static async Task<IResult> RequireAccessTokenEndpoint(
    [FromServices] IMediator mediator,
    HttpContext httpContext)
  {
    var userAgent = httpContext.Request.Headers.UserAgent;
    var refreshToken = httpContext.Request.Cookies["refresh_token"]?.ToString()
      ?? httpContext.Request.Query["refreshToken"].ToString();
    var request = new RequireAccessTokenCommand(refreshToken, userAgent!);
    var result = await mediator.Send(request);

    return GenerateHttpResponse(result);
  }

  private static async Task<IResult> SignoutEndpoint(
    [FromServices] IMediator mediator,
    HttpContext httpContext)
  {
    await mediator.Send(new RevokeTokenCommand());

    httpContext.Response.Cookies.Delete("refresh_token");
    httpContext.Response.Cookies.Delete("access_token");

    return TypedResults.NoContent();
  }

  private static IResult GenerateHttpResponse(Result result)
  {
    return result.IsSuccess ? TypedResults.Ok(result.Data) : TypedResults.BadRequest(result.Error);
  }
}
