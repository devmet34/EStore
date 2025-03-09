using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.App.Services;
/// <summary>
/// mc, First preference of service is cache then db. 
/// </summary>
public class BasketService : IBasketService
{

  /*
  private readonly TimeSpan cacheDuration = Constants.basketCacheDuration;

  private readonly ILogger<BasketCacheService> _logger;
  private readonly ProductService _productService;
  private readonly RedisService _redisService;
  private readonly IRepo<Basket> _repo;

  public BasketService(ILogger<BasketCacheService> logger, ProductService productService, RedisService redisService, IRepo<Basket> repo)
  {    
    _logger = logger;
    _productService = productService;
    _redisService = redisService;
    _repo = repo;
  }
  */

  private readonly IBasketCacheService _basketCacheSrv;
  private readonly IBasketDBService _basketDBSrv;
  private readonly ILogger<BasketService> _logger;
  private Basket? _basket;

  public BasketService(IBasketCacheService basketCacheSrv, IBasketDBService basketDBSrv, ILogger<BasketService> logger)
  {
    _basketCacheSrv = basketCacheSrv;
    _basketDBSrv = basketDBSrv;
    _logger = logger;
  }

  public Basket? GetBasket(string buyerId)
  {
    throw new NotImplementedException();
  }

  public Task<Basket?> GetBasketAsync(string buyerId, bool includeBasketItems = true, bool includeAll = false)
  {
    throw new NotImplementedException();
  }

  public async Task<Basket?> GetBasketAsync(string buyerId)
  {
    try { return await _basketCacheSrv.GetBasketAsync(buyerId) ?? await _basketDBSrv.GetBasketAsync(buyerId); }
    catch { return await _basketDBSrv.GetBasketAsync(buyerId); }

  }

  /// <summary>
  /// mc, Get/create basket from redis cache or db if cache throws error.
  /// </summary>
  /// <param name="buyerId"></param>
  /// <returns></returns>
  public async Task<Basket?> GetOrCreateBasketAsync(string buyerId)
  {
    try { return await _basketCacheSrv.GetOrCreateBasketAsync(buyerId); }
    catch { return await _basketDBSrv.GetOrCreateBasketAsync(buyerId); }
  }

  /// <summary>
  /// mc, Set basket firstly on redis cache then on db if cache throws error.
  /// </summary>
  /// <param name="buyerId"></param>
  /// <param name="productId"></param>
  /// <param name="qt"></param>
  /// <returns></returns>
  public async Task SetBasketItemAsync(string buyerId, int productId, int qt)
  {
    try
    {
      await _basketCacheSrv.SetBasketItemAsync(buyerId, productId, qt);
    }
    catch
    {
      await _basketDBSrv.SetBasketItemAsync(buyerId, productId, qt);
    }
  }

  public async Task RemoveBasketAsync(Basket basket)
  {
    try { await _basketCacheSrv.RemoveBasketAsync(basket); }
    catch { await _basketDBSrv.RemoveBasketAsync(basket); }
  }

  public async Task RemoveBasketItemAsync(string buyerId, int productId)
  {
    try { await _basketCacheSrv.RemoveBasketItemAsync(buyerId, productId); }
    catch { await _basketDBSrv.RemoveBasketItemAsync(buyerId, productId); }
  }


}
