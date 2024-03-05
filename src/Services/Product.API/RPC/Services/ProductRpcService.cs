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

  public override async Task<GetProductReply> GetProduct(GetProductDetailRequest request, ServerCallContext context)
  {
    var product = await _context.Products
      .AsNoTracking()
      .FirstOrDefaultAsync(e => e.Id == request.ProductId);

    if (product is null)
      return new GetProductReply();

    return new GetProductReply()
    {
      ProductId = product.Id,
      Name = product.Name,
      PictureUri = product.PictureUri,
      Price = product.Price,
      AvailableStock = product.AvailableStock,
      Description = product.Description,
    };
  }

  public override async Task<GetProductInBasketReply> GetProductInBasket(GetProductInBasketRequest request, ServerCallContext context)
  {
    var products = await _context.Products
      .AsNoTracking()
      .Where(e => request.ProductIds.Any(d => d == e.Id))
      .Select(e => new GetProductReply
      {
        ProductId = e.Id,
        Name = e.Name,
        PictureUri = e.PictureUri,
        Price = e.Price,
        AvailableStock = e.AvailableStock,
        Description = e.Description,
      })
      .ToListAsync();

    var reply = new GetProductInBasketReply();
    reply.Products.AddRange(products);

    return reply;
  }
}
