using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Product.API.Infrastructure.Database;
using Product.API.Infrastructure.Entities;

namespace Product.API.Endpoints;

[HttpPost("/api/products")]
[Authorize(Roles = "admin")]
public sealed class CreateProductEndpoint : Endpoint<CreateProductRequest, Result<ProductDetailResponse>>
{
  private readonly ProductDbContext _context;

  public CreateProductEndpoint(ProductDbContext context)
  {
    _context = context;
  }

  public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
  {
    var product = new ProductItem(
      name: req.Name,
      availableStock: req.AvailableStock,
      price: req.Price,
      pictureUri: req.PictureUri,
      brandId: req.BrandId,
      typeId: req.TypeId,
      description: req.Description);

    await _context.Products.AddAsync(product, ct);
    await _context.SaveChangesAsync(ct);

    // map entity to response
    var result = new ProductDetailResponse(
      Id: product.Id,
      Name: product.Name,
      Price: product.Price,
      AvailableStock: product.AvailableStock,
      PictureUri: product.PictureUri,
      BrandId: product.BrandId,
      TypeId: product.TypeId,
      Description: product.Description);

    await SendAsync(
      response: result,
      statusCode: 200,
      cancellation: ct);
  }
}
