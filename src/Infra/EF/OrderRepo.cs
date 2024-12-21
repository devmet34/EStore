using Estore.Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF;
public class OrderRepo
{

  private DbSet<Order> _context;

  public OrderRepo (EStoreDbContext context) {
    _context=context.Orders;
  }

  public async Task<IEnumerable<Order>> GetAllOrders(string buyerId)
  {
    return await _context.FromSql($"exec cus_sp_getorders {buyerId}").AsNoTracking().ToListAsync();
  }
  
}
