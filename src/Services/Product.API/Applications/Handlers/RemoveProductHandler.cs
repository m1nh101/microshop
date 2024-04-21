using API.Contract.Products.Requests;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;
using Product.API.Infrastructure.Entities;

namespace Product.API.Applications.Handlers;

public class RemoveProductHandler : IRequestHandler<RemoveProductRequest>
{
  private readonly ProductDbContext _context;

  public RemoveProductHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task<Result> Handle(RemoveProductRequest request)
  {
    var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == request.Id);
    if (product == null)
      return Result.Failed(Summary.NotFound, Error.NotFound<ProductItem>(request.Id));

    product.UpdateAvailableStatus(false);

    await _context.SaveChangesAsync();

    return Result.Ok();
  }
}