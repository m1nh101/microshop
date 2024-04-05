using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;
using Product.API.Infrastructure.Entities;

namespace Product.API.Handlers;

public class EditProductHandler : IRequestHandler<EditProductRequest>
{
  private readonly ProductDbContext _context;

  public EditProductHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task<object> Handle(EditProductRequest request)
  {
    var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == request.Id);
    if (product == null)
      return Errors.ProductNotFound;

    product.Update(new ProductItem(
      name: request.Name,
      availableStock: request.AvailableStock,
      price: request.Price,
      pictureUri: request.PictureUri,
      brandId: request.BrandId,
      typeId: request.TypeId,
      description: request.Description));

    await _context.SaveChangesAsync();

    var result = new ProductDetailResponse(
      Id: product.Id,
      Name: product.Name,
      Price: product.Price,
      AvailableStock: product.AvailableStock,
      PictureUri: product.PictureUri,
      BrandId: product.BrandId,
      TypeId: product.TypeId,
      Description: product.Description);

    return Result<ProductDetailResponse>.Ok(result);
  }
}
