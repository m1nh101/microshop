using API.Contract.Products.Responses;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Applications.Handlers;

public sealed record GetOptionRequest;

public class GetOptionHandler : IRequestHandler<GetOptionRequest>
{
  private readonly ProductDbContext _context;

  public GetOptionHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task<Result> Handle(GetOptionRequest request)
  {
    var brandOptions = await _context.Brands
      .AsNoTracking()
      .Select(e => new SelectOption
      {
        Label = e.Name,
        Value = e.Id
      })
      .ToListAsync();
    var typeOptions = await _context.Types
      .AsNoTracking()
      .Select(e => new SelectOption
      {
        Label = e.Name,
        Value = e.Id
      })
      .ToListAsync();
    var data = new FilterOptionResponse(brandOptions, typeOptions);

    return Result.Ok(data);
  }
}
