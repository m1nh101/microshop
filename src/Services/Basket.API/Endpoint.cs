using API.Contract.Baskets.Requests;
using API.Contract.Baskets.Responses;
using Basket.API.Handlers;
using Basket.API.Models;
using Common;
using Common.Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API;

public static class Endpoint
{
  public static IEndpointRouteBuilder UseBasketAPIEndpoint(this IEndpointRouteBuilder builder)
  {
    builder.MapGet("/api/baskets", GetBasketEndpoint).RequireAuthorization();
    builder.MapPost("/api/baskets", AddOrUpdateBasketItemEndpoint).RequireAuthorization();
    builder.MapDelete("/api/baskets/items/{id}", RemoveBasketItemEndpoint).RequireAuthorization();

    return builder;
  }

  private static async Task<Ok<Result<CustomerBasket>>> GetBasketEndpoint(
    [FromServices] IMediator mediator)
  {
    var query = new GetBasketRequest();
    var result = await mediator.Send(query).As<Result<CustomerBasket>>();

    return TypedResults.Ok(result);
  }

  private static async Task<Ok<Result<BasketChangedResponse>>> AddOrUpdateBasketItemEndpoint(
    [FromServices] IMediator mediator,
    [FromBody] AddOrUpdateBasketItemRequest request)
  {
    var result = await mediator.Send(request).As<Result<BasketChangedResponse>>();
    return TypedResults.Ok(result);
  }

  private static async Task<Ok<Result<BasketChangedResponse>>> RemoveBasketItemEndpoint(
    [FromServices] IMediator mediator,
    [FromRoute] string id)
  {
    var command = new RemoveBasketItemRequest(id);
    var result = await mediator.Send(command).As<Result<BasketChangedResponse>>();

    return TypedResults.Ok(result);
  }
}
