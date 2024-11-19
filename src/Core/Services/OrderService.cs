using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Entities.OrderAggregate;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using EStore.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Services;
public class OrderService
{
  private readonly IRepo<Order> _repo;
  private readonly ILogger<OrderService> _logger;
  private readonly BasketService _basketService;
  private readonly ProductService _productService;

  public OrderService(IRepo<Order> repo, ILogger<OrderService> logger, BasketService basketService, ProductService productService)
  {
    _repo = repo;
    _logger = logger;
    _basketService = basketService;
    _productService = productService;
  }

  public async Task<Order?> GetOrderAsync(int orderId )
  {
    return await _repo.Query().Where(o => o.Id == orderId).FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<Order?>> GetAllOrders(string buyerId)
  {
    return await _repo.Query().Where(o => o.BuyerId == buyerId)
      .Include(o=>o.OrderItems).AsNoTracking().ToListAsync();
  }

  public async Task CreateOrderAsync(Basket basket)
  {
    _logger.LogDebug("Creating order for user: "+basket.BuyerId);
    try
    {
      var order = new Order(basket);
      await _repo.AddAsync(order);
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }
    await _basketService.RemoveBasketAsync(basket!);

  }

  public async Task CreateOrderAsync(string buyerId )
  {
    var basket= await _basketService.GetBasketAsync(buyerId);
    basket.GuardNull();

    if (basket!.BasketItems.Count == 0)
      throw new Exception("Basket empty");

    foreach (var item in basket!.BasketItems)
    {
      if (await HasProductPriceUpdatedAsync(item))
        throw new Exception($"Product:{item.ProductName} price has changed");      
    }
    
    try
    {
      var order = new Order(basket!);
      await _repo.AddAsync(order);
    }
    catch (Exception ex) {
      throw new Exception( ex.Message);
    }
    await _basketService.RemoveBasketAsync(basket!);

  }

  private async Task<bool> HasProductPriceUpdatedAsync(BasketItem item)
  {
    var productPrice = await _productService.GetProductPriceAsync(item.ProductId);
    if (item.Price == productPrice)
      return false;
    return true;
  }






}
