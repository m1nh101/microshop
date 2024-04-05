using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using Common.Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Product.API.Handlers;

namespace Product.API;

public static class Endpoint
{
  public static IEndpointRouteBuilder UseProductAPIEndpoint(this IEndpointRouteBuilder builder)
  {
    builder.MapGet("/api/products/options", GetOptionEndpoint);
    builder.MapGet("/api/products/{id}", GetProductDetailEndpoint);
    builder.MapGet("/api/products", GetProductPaginationEndpoint);
    builder.MapPost("/api/products", CreateProductEndpoint).RequiredAdminRole();
    builder.MapPatch("/api/products/{id}", EditProductEndpoint).RequiredAdminRole();
    builder.MapDelete("/api/products/{id}", RemoveProductEndpoint).RequiredAdminRole();

    return builder;
  }

  private static RouteHandlerBuilder RequiredAdminRole(this RouteHandlerBuilder builder)
  {
    builder.RequireAuthorization(policy =>
    {
      policy.RequireAuthenticatedUser();
      policy.RequireRole("admin");
    });

    return builder;
  }

  private static async Task<Ok<Result<ProductDetailResponse>>> CreateProductEndpoint(
    [FromServices] IMediator mediator,
    [FromBody] CreateProductRequest request)
  {
    var result = await mediator.Send(request).As<Result<ProductDetailResponse>>();

    return TypedResults.Ok(result);
  }

  private static async Task<Ok<Result<ProductDetailResponse>>> EditProductEndpoint(
    [FromServices] IMediator mediator,
    [FromBody] EditProductRequest request,
    [FromRoute] string id)
  {
    var result = await mediator.Send(request).As<Result<ProductDetailResponse>>();

    return TypedResults.Ok(result);
  }

  private static async Task<Ok<Result<RemoveProductResponse>>> RemoveProductEndpoint(
    [FromServices] IMediator mediator,
    [FromRoute] string id)
  {
    var command = new RemoveProductRequest() { Id = id };
    var result = await mediator.Send(command).As<Result<RemoveProductResponse>>();

    return TypedResults.Ok(result);
  }

  private static async Task<Ok<Result<FilterOptionResponse>>> GetOptionEndpoint(
    [FromServices] IMediator mediator)
  {
    var query = new GetOptionRequest();
    var result = await mediator.Send(query).As<Result<FilterOptionResponse>>();

    return TypedResults.Ok(result);
  }

  private static async Task<Ok<Result<ProductDetailResponse>>> GetProductDetailEndpoint(
    [FromServices] IMediator mediator,
    [FromRoute] string id)
  {
    var query = new GetProductByIdRequest { Id = id };
    var result = await mediator.Send(query).As<Result<ProductDetailResponse>>();
    return TypedResults.Ok(result);
  }

  private static async Task<Ok<Result<IEnumerable<ProductPaginationResponse>>>> GetProductPaginationEndpoint(
    [FromServices] IMediator mediator,
    [FromQuery] GetProductPaginationRequest request)
  {
    var result = await mediator.Send(request).As<Result<IEnumerable<ProductPaginationResponse>>>();
    return TypedResults.Ok(result);
  }
}
