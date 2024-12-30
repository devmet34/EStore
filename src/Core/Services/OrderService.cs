using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Entities.OrderAggregate;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Estore.Core.Services;
public class OrderService
{
  private readonly IRepo<Order> _repo;
  private readonly ILogger<OrderService> _logger;
  private readonly IBasketService _basketService;
  private readonly ProductService _productService;
  private readonly IRepoOrder _repoOrder;
  public OrderService(IRepo<Order> repo, IRepoOrder repoOrder, ILogger<OrderService> logger, Interfaces.IBasketService basketService, ProductService productService)
  {
    _repo = repo;
    _repoOrder = repoOrder;
    _logger = logger;
    _basketService = basketService;
    _productService = productService;
  }

  public async Task<Order?> GetOrderAsync(int orderId )
  {
    return await _repo.Query().Where(o => o.Id == orderId).FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<Order?>> GetAllOrdersAsync(string buyerId)
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

    try
    {
      await _repoOrder.CreateOrderAsync(basket);
      await _basketService.RemoveBasketAsync(basket!);
      return;
    }
    catch (Exception ex) {
      _logger.LogError(ex.Message);
      throw;
     }
    
    

    foreach (var item in basket!.BasketItems)
    {
      if (await HasProductPriceUpdatedAsync(item))
        throw new Exception($"Product:{item.Product?.Name} price has changed");      
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
    if (item.Product?.Price == productPrice)
      return false;
    return true;
  }






}
