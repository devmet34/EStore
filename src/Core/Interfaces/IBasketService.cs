using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Models;

namespace EStore.Core.Interfaces;
public interface IBasketService
{
  /// <summary>
  /// mc, If basket not already created, create it.
  /// </summary>
  /// <param name="buyerId"></param>
  /// <returns></returns>
  public Task CreateBasketAsync(string buyerId);
  public Task<Basket?> GetBasketAsync(string buyerId);
  public Task<BasketVM?> GetBasketVMAsync(string buyerId);
  public Task<int> GetBasketCountAsync(string buyerId);
  public Task SetBasketItemAsync(string buyerId, int productId, int qt);
  //public Task SetBasketCountAsync(string buyerId);
  public Task RemoveBasketItemAsync(string buyerId, int productId);
  public Task RemoveBasketAsync(string buyerId);




}
