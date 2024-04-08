using API.Contract.Baskets.Requests;
using API.Contract.Baskets.Responses;
using Basket.API.Applications.Handlers;
using Basket.API.Models;
using Common;
using Common.Mediator;
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

  private static async Task<IResult> GetBasketEndpoint(
    [FromServices] IMediator mediator)
  {
    var query = new GetBasketRequest();
    var result = await mediator.Send(query);

    return GenerateHttpResponse(result);
  }

  private static async Task<IResult> AddOrUpdateBasketItemEndpoint(
    [FromServices] IMediator mediator,
    [FromBody] AddOrUpdateBasketItemRequest request)
  {
    var result = await mediator.Send(request);
    return GenerateHttpResponse(result);
  }

  private static async Task<IResult> RemoveBasketItemEndpoint(
    [FromServices] IMediator mediator,
    [FromRoute] string id)
  {
    var command = new RemoveBasketItemRequest(id);
    var result = await mediator.Send(command);

    return GenerateHttpResponse(result);
  }

  private static IResult GenerateHttpResponse(Result result)
  {
    return result.IsSuccess ? TypedResults.Ok(result.Data) : TypedResults.BadRequest(result.Errors);
  }
}
