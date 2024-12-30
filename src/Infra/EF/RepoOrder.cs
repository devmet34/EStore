using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Entities.OrderAggregate;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF;
public class RepoOrder:IRepoOrder
{

  private EStoreDbContext _context;

  public RepoOrder (EStoreDbContext context) {
    _context=context;
  }

  public async Task<IEnumerable<Order?>> GetAllOrdersAsync(string buyerId)
  {
    buyerId.GuardNullOrEmpty();
    return await _context.Orders.Where(o => o.BuyerId == buyerId)
      .Include(o => o.OrderItems).AsNoTracking().ToListAsync();
  }

  public async Task CreateOrderAsync(Basket basket)
  {
    basket.GuardNull();
    basket.BasketItems.GuardNull();

    bool hasOrderCached = false;
    bool saved = false;
    bool concurrencyException = false;
    while (!saved)
    {
      
      foreach (var item in basket.BasketItems)
      {
        var dbItem = await _context.Products.Where(p => p.Id == item.ProductId).FirstOrDefaultAsync();
        dbItem.GuardNull();
        //mc if concurrencyException was thrown and item state modified, reload ef tracked/cached entity from db. It looks like even rows/items not changed seem changed probably because it belongs to entity type that changed, bug? or expected behaviour?  
        if (concurrencyException) 
        {
          var entry = _context.Entry(dbItem!);
          if (entry.State == EntityState.Modified)
            await entry.ReloadAsync();
          else
            continue;
        }
        
         
        
        if (item.Product?.Price != dbItem!.Price)
          throw new Exception($"Price of product {item.Product?.Name} changed");
        if (dbItem!.Qt < item.Qt)
          throw new Exception($"Product {item.Product.Name} quantity not enough");

        dbItem.UpdateQt(-item.Qt);
      }

      //mc to prevent creating duplicate orders in case of concurrency exception loops
      if (!hasOrderCached) 
      {
        var order = new Order(basket);
        _context.Orders.Add(order);
        hasOrderCached = true;
      }

      try { 
        await _context.SaveChangesAsync();
        saved = true;
      }
      catch (DbUpdateConcurrencyException ex)
      {
        concurrencyException = true;
        //foreach (var entry in ex.Entries) //mc this only returns single entry, a bug?
        //entry.Reload();

      }
      
    }

  }

  public Task<Order?> GetOrderAsync(int orderId)
  {
    throw new NotImplementedException();
  }
}//eo class
