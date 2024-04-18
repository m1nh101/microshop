using Common.Contracts;

namespace Product.API.Infrastructure.Entities;

public class ProductSize : IIdentity<string>
{
  private ProductSize() { }

  public ProductSize(string size, SizeGenre genre)
  {
    Id = Guid.NewGuid().ToString();
    Size = size;
    Genre = genre;
  }

  public string Id { get; private set; } = string.Empty;
  public string Size { get; private set; } = string.Empty;
  public SizeGenre Genre { get; private set; }

  public virtual ICollection<ProductUnit> Units { get; set; } = null!;
}


public enum SizeGenre : byte
{
  Kid = 0,
  Men = 1,
  Women = 2
}