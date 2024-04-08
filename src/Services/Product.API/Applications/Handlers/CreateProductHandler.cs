using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using Common.Mediator;
using Product.API.Infrastructure.Database;
using Product.API.Infrastructure.Entities;

namespace Product.API.Applications.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductRequest>
{
  private readonly ProductDbContext _context;

  public CreateProductHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task<Result> Handle(CreateProductRequest request)
  {
    var product = new ProductItem(
      name: request.Name,
      availableStock: request.AvailableStock,
      price: request.Price,
      pictureUri: request.PictureUri,
      brandId: request.BrandId,
      typeId: request.TypeId,
      description: request.Description);

    await _context.Products.AddAsync(product);
    await _context.SaveChangesAsync();

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

    return Result.Ok(result);
  }
}
