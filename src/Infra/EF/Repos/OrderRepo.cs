using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Entities.OrderAggregate;
using EStore.Core.Extensions;
using EStore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Repos;
public class OrderRepo : IRepoOrder
{

    private EStoreDbContext _context;

    public OrderRepo(EStoreDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetOrderAsync(int orderId)
    {
        orderId.GuardZero();
        return await _context.Orders.Where(o => o.Id == orderId).AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order?>> GetAllOrdersAsync(string buyerId)
    {
        buyerId.GuardNullOrEmpty();
        return await _context.Orders.Where(o => o.BuyerId == buyerId)
          .Include(o => o.OrderItems).AsNoTracking().ToListAsync();
    }
    /// <summary>
    ///mc, Create order async with optimistic concurrency handling.
    /// </summary>
    /// <param name="basket"></param>
    /// <param name="addressId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task CreateOrderAsync(Basket basket, int addressId)
    {
        basket.GuardNull();
        basket.BasketItems.GuardNull();
        addressId.GuardZero();

        bool hasOrderCached = false;
        bool saved = false;
        bool concurrencyException = false;
        //mc, Saved flag will be false until order created/saved to db. When there is a change in related data, loop will start again. Within the loop, items (product) will be checked for price/qt etc. constraints. 
        while (!saved)
        {
              
            foreach (var basketItem in basket.BasketItems)
            {
                //todo ef doesnt track projections thus no concurrency protection take place if used, any workaround maybe table splitting?
                var productOnDB = await _context.Products.Where(p => p.Id == basketItem.ProductId).FirstOrDefaultAsync();
                productOnDB.GuardNull();
                //mc, if concurrencyException was thrown and item state modified, reload ef tracked/cached entity from db. It looks like even rows/items not changed seem changed probably because it belongs to entity type that changed, bug? or expected behaviour?  
                if (concurrencyException)
                {
                    var entry = _context.Entry(productOnDB!);
                    if (entry.State == EntityState.Modified)
                        await entry.ReloadAsync();
                    else
                        continue;
                }

                if (basketItem.Product?.Price != productOnDB!.Price)
                    throw new Exception($"Price of product {basketItem.Product?.Name} changed");
                if (productOnDB!.Qt < basketItem.Qt)
                    throw new Exception($"Product {basketItem.Product.Name} quantity not enough");
                //mc, decrease product qt by basket item qt 
                productOnDB.UpdateQt(-basketItem.Qt);
            }

            //mc, to prevent creating duplicate orders
            if (!hasOrderCached)
            {
                var order = new Order(basket, addressId);
                _context.Orders.Add(order);
                hasOrderCached = true;
            }

            try
            {
                await _context.SaveChangesAsync();
                saved = true;
            }
            catch (DbUpdateConcurrencyException)
            {
                concurrencyException = true;
                //foreach (var entry in ex.Entries) //mc, this only returns single entry, a bug?
                //entry.Reload();

            }

        }

    }



}//eo class
