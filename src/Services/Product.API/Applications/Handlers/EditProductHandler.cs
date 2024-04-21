using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;
using Product.API.Infrastructure.Entities;

namespace Product.API.Applications.Handlers;

public class EditProductHandler : IRequestHandler<EditProductRequest>
{
  private readonly ProductDbContext _context;

  public EditProductHandler(ProductDbContext context)
  {
    _context = context;
  }

  public async Task<Result> Handle(EditProductRequest request)
  {
    // validate input vs record in db
    var product = await _context.Products
      .Include(e => e.Units)
      .Include(e => e.Categories)
      .Include(e => e.Brand)
      .FirstOrDefaultAsync(e => e.Id == request.Id);
    if (product == null)
      return Result.Failed(Summary.NotFound, Error.NotFound<ProductItem>(request.Id));

    var errors = new List<Error>();
    var sizePayloads = request.Unit.Add.Select(x => x.SizeId);
    var colorPayloads = request.Unit.Add.Select(x => x.ColorId);
    var brand = await _context.Brands.FirstOrDefaultAsync(e => e.Id == request.BrandId);
    var categories = await _context.Categories
      .Where(e => request.Categories.Add.Any(d => d == e.Id)).ToArrayAsync();
    var sizes = await _context.Sizes
      .Where(d => sizePayloads.Any(f => d.Id == f)).ToListAsync();
    var colors = await _context.Colors
       .Where(d => colorPayloads.Any(f => d.Id == f)).ToListAsync();

    ProductCollection? collection = null;
    if (!string.IsNullOrEmpty(request.Collection))
    {
      collection = await _context.Collections.FirstOrDefaultAsync(e => e.Id == request.Collection);

      if (collection is null)
        errors.Add(Error.NotFound<ProductCollection>(request.Collection));
    }

    if (brand is null)
      errors.Add(Error.NotFound<ProductBrand>(request.BrandId));

    if (categories is null)
      errors.AddRange(Error.NotFound<Category>(request.Categories.Add.Select(d => d)));

    if (sizes is null)
      errors.AddRange(Error.NotFound<ProductSize>(sizePayloads));

    if (colors is null)
      errors.AddRange(Error.NotFound<ProductColor>(colorPayloads));

    if (errors.Count > 0)
      return Result.ValidateFailed([.. errors]);

    // update to product
    var unitsToUpdate = request.Unit.Edit.ToDictionary(e => e.Id, e => (e.Price, e.Stock));

    foreach (var payload in request.Unit.Add)
    {
      var size = sizes!.First(e => e.Id == payload.SizeId);
      var color = colors!.First(e => e.Id == payload.ColorId);

      product.AddUnit(new ProductUnit(payload.Stock, payload.Price, size, color));
    }

      product.AssignToCategories(categories!)
        .RemoveAssignedCategories(request.Categories.Remove)
        .UpdateUnits(unitsToUpdate, out ICollection<Error> unitUpdateError)
        .RemoveUnit(request.Unit.Remove, out ICollection<Error> unitRemoveError);

    await _context.SaveChangesAsync();

    var errorAfterUpdateSuccess = unitUpdateError.Concat(unitRemoveError).ToList();

    var response = new ProductUpdatedSuccessfullyResponse
    {
      Id = product.Id,
      ModifiedAt = product.ModifiedAt!.Value,
      UnitUpdateErrors = errorAfterUpdateSuccess
    };

    return Result.Ok(response);
  }
}
