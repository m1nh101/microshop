using API.Contract.Baskets.Requests;
using Basket.API.Applications.Handlers;
using Common.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API;

public static class Endpoint
{
  public static IEndpointRouteBuilder UseBasketAPIEndpoint(this IEndpointRouteBuilder builder)
  {
    builder.MapGet("/api/baskets", GetBasketEndpoint).RequireAuthorization();
    builder.MapPost("/api/baskets", AddOrUpdateBasketItemEndpoint).RequireAuthorization();
    builder.MapDelete("/api/baskets/items/{productId}/units/{unitId}", RemoveBasketItemEndpoint).RequireAuthorization();

    return builder;
  }

  private static async Task<IResult> GetBasketEndpoint(
    [FromServices] IMediator mediator)
  {
    var query = new GetBasketRequest();
    var result = await mediator.Send(query);

    return result.IsSuccess
      ? TypedResults.Ok(result.Data)
      : TypedResults.BadRequest(result.Error);
  }

  private static async Task<IResult> AddOrUpdateBasketItemEndpoint(
    [FromServices] IMediator mediator,
    [FromBody] AddOrUpdateBasketItemRequest request)
  {
    var result = await mediator.Send(request);

    return result.IsSuccess
      ? TypedResults.Created("/api/baskets", result.Data)
      : TypedResults.BadRequest(result.Error);
  }

  private static async Task<IResult> RemoveBasketItemEndpoint(
    [FromServices] IMediator mediator,
    [FromRoute] string productId, string unitId)
  {
    var command = new RemoveBasketItemRequest(productId, unitId);
    var result = await mediator.Send(command);

    return result.IsSuccess
      ? TypedResults.NoContent()
      : TypedResults.BadRequest(result.Error);
  }
}
