using Common;
using Common.Mediator;
using Microsoft.AspNetCore.Mvc;
using Order.API.Applications.Orders.Handlers;
using Order.API.Applications.Orders.Responses;

namespace Order.API;

public static class Endpoint
{
  public static IEndpointRouteBuilder UseOrderAPIEndpoint(this IEndpointRouteBuilder builder)
  {
    builder.MapPost("/api/orders", CreateOrderEndpoint).RequireAuthorization();
    builder.MapDelete("/api/orders/{orderId}", CancelOrderEndpoint).RequireAuthorization();
    builder.MapGet("/api/orders", GetUserOrderEndpoint).RequireAuthorization();
    builder.MapGet("/api/admin/orders", GetOrderEndpoint).RequireAuthorization(policy =>
    {
      policy.RequireRole("admin");
    });
    builder.MapGet("/api/orders/{orderId}", GetOrderDetail).RequireAuthorization();

    return builder;
  }

  private static async Task<IResult> CreateOrderEndpoint(
    [FromServices] IMediator mediator,
    [FromBody] CreateOrderRequest request)
  {
    var result = await mediator.Send(request);

    return GenerateHttpResponse(result);
  }

  private static async Task<IResult> CancelOrderEndpoint(
    [FromServices] IMediator mediator,
    [FromRoute] string orderId)
  {
    var command = new CancelOrderRequest(orderId);

    await mediator.Send(command);

    return TypedResults.NoContent();
  }

  private static async Task<IResult> GetOrderEndpoint(
    [FromServices] IMediator mediator)
  {
    var query = new GetOrderRequest();
    var result = await mediator.Send(query);

    return GenerateHttpResponse(result);
  }

  private static async Task<IResult> GetUserOrderEndpoint(
    [FromServices] IMediator mediator)
  {
    var query = new GetUserOrderRequest();
    var result = await mediator.Send(query);

    return GenerateHttpResponse(result);
  }

  private static async Task<IResult> GetOrderDetail(
    [FromServices] IMediator mediator,
    [FromRoute] string orderId)
  {
    var query = new GetOrderDetailRequest(orderId);
    var result = await mediator.Send(query);

    return GenerateHttpResponse(result);
  }

  private static IResult GenerateHttpResponse(Result result)
  {
    return result.IsSuccess ? TypedResults.Ok(result.Data) : TypedResults.BadRequest(result.Errors);
  }
}
