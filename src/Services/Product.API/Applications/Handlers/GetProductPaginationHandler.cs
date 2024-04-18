using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Applications.Handlers;

public class GetProductPaginationHandler : IRequestHandler<GetProductPaginationRequest>
{
  private readonly ProductDbContext _context;
  private const int NumberOfItemPerPage = 25;

  public GetProductPaginationHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task<Result> Handle(GetProductPaginationRequest request)
  {
    var query = _context.Products
      .Include(e => e.Brand)
      .AsNoTracking();
    var brands = request.Brands?.Where(e => !string.IsNullOrEmpty(e)).ToList();

    if (!string.IsNullOrEmpty(request.Name))
      query = query.Where(e => e.Name.StartsWith(request.Name));
    if (brands != null && brands.Count > 0)
      query = query.Where(e => brands.Any(d => d == e.BrandId));

    var products = await query
      .Skip(request.PageIndex * NumberOfItemPerPage)
      .Take(NumberOfItemPerPage)
      .Select(e => new ProductPaginationResponse(
        e.Id,
        e.Name,
        e.Price,
        e.Brand.Name,
        e.PictureUri,
        e.Description))
      .ToListAsync();

    return Result.Ok(products);
  }
}
