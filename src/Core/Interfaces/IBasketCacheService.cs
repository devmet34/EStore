namespace EStore.Core.Interfaces;
public interface IBasketCacheService : IBasketService
{
  public Task SetBasketCountAsync(string buyerId, int count);
}
