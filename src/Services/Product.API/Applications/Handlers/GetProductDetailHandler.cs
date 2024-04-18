using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Applications.Handlers;

public class GetProductDetailHandler : IRequestHandler<GetProductByIdRequest>
{
  private readonly ProductDbContext _context;

  public GetProductDetailHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task<Result> Handle(GetProductByIdRequest request)
  {
    var product = await _context.Products
      .AsNoTracking()
      .Include(e => e.Brand)
      .Include(e => e.Categories)
      .Where(e => e.Id == request.Id)
      .Include(e => e.Units)
      .ThenInclude(e => e.Color)
      .Include(e => e.Units)
      .ThenInclude(e => e.Size)
      .Select(e => new ProductDetailResponse
      {
        Id = e.Id,
        Name = e.Name,
        Price = e.Price,
        Picture = e.PictureUri,
        BrandId = e.BrandId,
        BrandName = e.Brand.Name,
        Material = e.Material,
        Description = e.Description,
        Units = e.Units.Select(d => new UnitDetail(d.Id, d.SizeId, d.Size.Size, d.ColorId, d.Color.Name, d.Price, d.Stock))
      })
      .FirstOrDefaultAsync(e => e.Id == request.Id);

    if (product is null)
      return Result.Failed(Errors.ProductNotFound);

    return Result.Ok(product);
  }
}
