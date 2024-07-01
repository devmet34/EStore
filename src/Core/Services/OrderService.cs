using Estore.Core.Entities;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Services;
public class OrderService
{
  private readonly IRepo<Order> _repo;
  private readonly ILogger<OrderService> _logger;
  private readonly BasketService _basketService;

  public OrderService(IRepo<Order> repo, ILogger<OrderService> logger, BasketService basketService)
  {
    _repo = repo;
    _logger = logger;
    _basketService = basketService;
  }

  public async Task<Order?> GetOrderAsync(int orderId )
  {
    return await _repo.Query().Where(o => o.Id == orderId).FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<Order?>> GetAllOrders(string buyerId)
  {
    return await _repo.Query().Where(o => o.BuyerId == buyerId).ToListAsync();
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

  public async Task CreateOrderAsync(string buyerId)
  {
    var basket= await _basketService.GetBasketAsync(buyerId);
    basket.GuardNull();
    
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



  


}
