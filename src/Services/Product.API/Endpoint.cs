﻿using API.Contract.Common;
using API.Contract.Products.Requests;
using Common.Mediator;
using Microsoft.AspNetCore.Mvc;
using Product.API.Applications.Handlers;

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

  private static async Task<IResult> CreateProductEndpoint(
    [FromServices] IMediator mediator,
    [FromBody] CreateProductRequest request)
  {
    var result = await mediator.Send(request);

    return result.IsSuccess
      ? TypedResults.Created($"/api/products/{result.As<ResourceCreateSuccessfulResponse>().Id}", result.Data)
      : TypedResults.BadRequest(result.Error);
  }

  private static async Task<IResult> EditProductEndpoint(
    [FromServices] IMediator mediator,
    [FromBody] EditProductRequest request,
    [FromRoute] string id)
  {
    var result = await mediator.Send(request);

    return result.IsSuccess
      ? TypedResults.Ok(result.Data)
      : TypedResults.BadRequest(result.Error);
  }

  private static async Task<IResult> RemoveProductEndpoint(
    [FromServices] IMediator mediator,
    [FromRoute] string id)
  {
    var command = new RemoveProductRequest() { Id = id };
    var result = await mediator.Send(command);

    return result.IsSuccess
      ? TypedResults.NoContent()
      : TypedResults.BadRequest(result.Error);
  }

  private static async Task<IResult> GetOptionEndpoint(
    [FromServices] IMediator mediator)
  {
    var query = new GetOptionRequest();
    var result = await mediator.Send(query);

    return TypedResults.Ok(result.Data);
  }

  private static async Task<IResult> GetProductDetailEndpoint(
    [FromServices] IMediator mediator,
    [FromRoute] string id)
  {
    var query = new GetProductByIdRequest { Id = id };
    var result = await mediator.Send(query);

    return result.IsSuccess
      ? TypedResults.Ok(result.Data)
      : TypedResults.NotFound(result.Error);
  }

  private static async Task<IResult> GetProductPaginationEndpoint(
    [FromServices] IMediator mediator,
    [FromQuery] string[]? brands,
    [FromQuery] string? typeId,
    [FromQuery] string? name,
    [FromQuery] int pageIndex = 0)
  {
    var query = new GetProductPaginationRequest()
    {
      PageIndex = pageIndex,
      Brands = brands,
      Name = name,
      TypeId = typeId,
    };
    var result = await mediator.Send(query);

    return result.IsSuccess 
      ? TypedResults.Ok(result.Data)
      : TypedResults.BadRequest(result.Error);
  }
}
