using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Handlers;

public class GetProductDetailHandler : IRequestHandler<GetProductByIdRequest>
{
  private readonly ProductDbContext _context;

  public GetProductDetailHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task<object> Handle(GetProductByIdRequest request)
  {
    var product = await _context.Products
      .AsNoTracking()
      .Where(e => e.Id == request.Id)
      .Select(e => new ProductDetailResponse(
        e.Id,
        e.Name,
        e.Price,
        e.AvailableStock,
        e.PictureUri,
        e.BrandId,
        e.TypeId,
        e.Description))
      .FirstOrDefaultAsync();

    if (product is null)
      return Errors.ProductNotFound;

    return Result<ProductDetailResponse>.Ok(product);
  }
}
