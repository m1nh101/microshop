using API.Contract.Common;
using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;
using Product.API.Infrastructure.Entities;

namespace Product.API.Applications.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductRequest>
{
  private readonly ProductDbContext _context;

  public CreateProductHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task<Result> Handle(CreateProductRequest request)
  {
    var errors = new List<Error>();
    var sizePayloads = request.Units.Select(x => x.SizeId);
    var colorPayloads = request.Units.Select(x => x.ColorId);
    var brand = await _context.Brands.FirstOrDefaultAsync(e => e.Id == request.BrandId);
    var categories = await _context.Categories
      .Where(e => request.Categories.Add.Any(d => d == e.Id)).ToArrayAsync();
    var sizes = await _context.Sizes
      .Where(d => sizePayloads.Any(f => d.Id == f)).ToListAsync();
    var colors = await _context.Colors
       .Where(d => colorPayloads.Any(f => d.Id == f)).ToListAsync();

    ProductCollection? collection = null;
    if (!string.IsNullOrEmpty(request.Collection)) {
      collection = await _context.Collections.FirstOrDefaultAsync(e => e.Id == request.Collection);

      if (collection is null)
        errors.Add(Error.NotFound<ProductCollection>(request.Collection));
    }

    if (brand is null)
      errors.Add(Error.NotFound<ProductBrand>(request.BrandId));
    
    if(categories is null)
      errors.AddRange(Error.NotFound<Category>(request.Categories.Add.Select(d => d)));

    if (sizes is null)
      errors.AddRange(Error.NotFound<ProductSize>(sizePayloads));

    if (colors is null)
      errors.AddRange(Error.NotFound<ProductColor>(colorPayloads));

    if (errors.Count > 0)
      return Result.ValidateFailed([.. errors]);

    var product = new ProductItem(
      name: request.Name,
      price: request.Price,
      picture: request.PictureUri,
      brand: brand!,
      material: request.Material,
      collection: collection?.Id,
      description: request.Description)
      .AssignToCategories(categories!);

    foreach(var payload in request.Units)
    {
      var size = sizes!.First(e => e.Id == payload.SizeId);
      var color = colors!.First(e => e.Id == payload.ColorId);

      product.AddUnit(new ProductUnit(payload.Stock, payload.Price, size, color));
    }

    await _context.Products.AddAsync(product);
    await _context.SaveChangesAsync();

    // map entity to response
    var response = new ResourceCreateSuccessfulResponse
    {
      Id = product.Id,
      CreatedAt = product.CreatedAt
    };

    return Result.Ok(response);
  }
}
