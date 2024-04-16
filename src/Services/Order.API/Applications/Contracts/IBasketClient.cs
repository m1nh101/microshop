using API.Contract.Baskets.Models;

namespace Order.API.Applications.Contracts;

public interface IBasketClient
{
  Task<IEnumerable<BasketItem>> GetBasket(string userId);
}