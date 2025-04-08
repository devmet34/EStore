using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Entities.OrderAggregate;

namespace EStore.Core.Interfaces;
public interface IRepoOrder
{
  public Task<Order?> GetOrderAsync(int orderId);
  public Task<IEnumerable<Order?>> GetAllOrdersAsync(string buyerId);
  public Task CreateOrderAsync(Basket basket, int addressId);
}
