using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Interfaces;
public interface IRepoOrder
{
  public Task<Order?> GetOrderAsync(int orderId);
  public Task<IEnumerable<Order?>> GetAllOrdersAsync(string buyerId);
  public Task CreateOrderAsync(Basket basket);
}
