using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Applications.Handlers;

public class RemoveProductHandler : IRequestHandler<RemoveProductRequest>
{
  private readonly ProductDbContext _context;

  public RemoveProductHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task<object> Handle(RemoveProductRequest request)
  {
    var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == request.Id);
    if (product == null)
      return Errors.ProductNotFound;

    product.UpdateAvailableStatus(false);

    await _context.SaveChangesAsync();

    return Result<RemoveProductResponse>.Ok(new RemoveProductResponse());
  }
}