using Common;
using FastEndpoints;
using Product.API.Http.Requests;
using Product.API.Http.Responses;
using Product.API.Infrastructure.Database;
using Product.API.Infrastructure.Entities;

namespace Product.API.Http.Endpoints;

[HttpPost("/api/products")]
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
      Description: product.Description);

    await SendAsync(
      response: result,
      statusCode: 200,
      cancellation: ct);
  }
}
