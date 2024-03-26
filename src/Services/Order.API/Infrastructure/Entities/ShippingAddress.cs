namespace Order.API.Infrastructure.Entities;

public class ShippingAddress
{
  private ShippingAddress() { }

  public ShippingAddress(string address, string province, string district, string ward)
  {
    Address = address;
    Province = province;
    District = district;
    Ward = ward;
  }

  public string Address { get; private set; } = string.Empty;
  public string District { get; private set; } = string.Empty;
  public string Province { get; private set; } = string.Empty;
  public string Ward { get; private set; } = string.Empty;
}