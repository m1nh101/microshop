using API.Contract.Products.Responses;
using Common;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

namespace Product.API.Endpoints;

[HttpGet("/api/products/list-option")]
[AllowAnonymous]
public class GetOptionListEndpoint : Endpoint<EmptyRequest, Result<FilterOptionResponse>>
{
  private readonly ProductDbContext _context;

  public GetOptionListEndpoint(ProductDbContext context)
  {
    _context = context;
  }

  public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
  {
    var brandOptions = await _context.Brands
      .AsNoTracking()
      .Select(e => new SelectOption
      {
        Label = e.Name,
        Value = e.Id
      })
      .ToListAsync(ct);
    var typeOptions = await _context.Types
      .AsNoTracking()
      .Select(e => new SelectOption
      {
        Label = e.Name,
        Value = e.Id
      })
      .ToListAsync(ct);
    var data = new FilterOptionResponse(brandOptions, typeOptions);

    await SendAsync(
      response: Result<FilterOptionResponse>.Ok(data),
      statusCode: 200,
      cancellation: ct);
  }
}
