using API.Contract.Products.Requests;
using API.Contract.Products.Responses;
using Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;
using Product.API.Infrastructure.Entities;

namespace Product.API.Endpoints;

[HttpPut("/api/products/{id}")]
public sealed class EditProductEndpoint : Endpoint<EditProductRequest, Result<ProductDetailResponse>>
{
    private readonly ProductDbContext _context;

    public EditProductEndpoint(ProductDbContext context)
    {
        _context = context;
    }

    public override async Task HandleAsync(EditProductRequest req, CancellationToken ct)
    {
        var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == req.Id, ct);

        if (product == null)
        {
            await SendAsync(
              response: Errors.ProductNotFound,
              statusCode: 400,
              cancellation: ct);
            return;
        }

        product.Update(new ProductItem(
          name: req.Name,
          availableStock: req.AvailableStock,
          price: req.Price,
          pictureUri: req.PictureUri,
          brandId: req.BrandId,
          typeId: req.TypeId,
          description: req.Description));

        await _context.SaveChangesAsync(ct);

        var result = new ProductDetailResponse(
          Id: product.Id,
          Name: product.Name,
          Price: product.Price,
          AvailableStock: product.AvailableStock,
          PictureUri: product.PictureUri,
          Description: product.Description);

        await SendAsync(
          response: result,
          statusCode: 200,
          cancellation: ct);
    }
}
