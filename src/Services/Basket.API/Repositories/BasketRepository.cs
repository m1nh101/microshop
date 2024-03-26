using Basket.API.Models;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace Basket.API.Repositories;

public interface IBasketRepository
{
  Task<CustomerBasket?> GetBasket(string customerId);
  Task UpdateBasket(CustomerBasket basket);
  Task RemoveBasket(string customerId);
}

public class BasketRepository : IBasketRepository
{
  private readonly IRedisCollection<CustomerBasket> _baskets;

  public BasketRepository(IRedisConnectionProvider provider)
  {
    _baskets = provider.RedisCollection<CustomerBasket>();
  }

  public async Task<CustomerBasket?> GetBasket(string customerId)
  {
    return await _baskets.FindByIdAsync(customerId);
  }

  public async Task RemoveBasket(string customerId)
  {
    var basket = await GetBasket(customerId);
    if (basket is not null)
      await _baskets.DeleteAsync(basket);
  }

  public async Task UpdateBasket(CustomerBasket customer)
  {
    await _baskets.InsertAsync(customer);
  }
}
