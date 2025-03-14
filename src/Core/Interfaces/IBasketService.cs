using EStore.Core.Entities.BasketAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Interfaces;
public interface IBasketService
{
  public Task<Basket?> GetOrCreateBasketAsync(string buyerId);

  public Task<Basket?> GetBasketAsync(string buyerId, bool includeBasketItems = true, bool includeAll = false);
  public Task<Basket?> GetBasketAsync(string buyerId);
  public Basket? GetBasket(string buyerId);
  public Task SetBasketItemAsync(string buyerId, int productId, int qt);
  public Task RemoveBasketItemAsync(string buyerId, int productId);
  public Task RemoveBasketAsync(Basket basket);




}
