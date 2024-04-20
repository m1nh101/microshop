using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.RPC.Services;

public class ProductRpcService : ProductRpc.ProductRpcBase
{
  private readonly ProductDbContext _context;

  public ProductRpcService(ProductDbContext context)
  {
    _context = context;
  }

  public override async Task<UnitInformationMessageReply> GetProductUnitInformation(ProductUnitMessageRequest request, ServerCallContext context)
  {
    var unit = await _context.Products
      .AsNoTracking()
      .Include(e => e.Units)
      .ThenInclude(e => e.Size)
      .Include(e => e.Units)
      .ThenInclude(e => e.Color)
      .Where(e => e.Id == request.ProductId)
      .SelectMany(e => e.Units, (p, u) => new UnitInformationMessageReply
      {
        ProductId = p.Id,
        UnitId = u.Id,
        Name = p.Name,
        Price = p.Price + u.Price,
        Stock = u.Stock,
        Color = u.Color.Name,
        Size = u.Size.Size,
        Picture = p.PictureUri
      })
      .FirstOrDefaultAsync(e => e.UnitId == request.UnitId);


    return unit ?? new UnitInformationMessageReply();
  }
}
