using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.App.Services;
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

  /// <summary>
  /// mc, Get basket from cache, try to get it from DB if cache fails.
  /// </summary>
  /// <param name="buyerId"></param>
  /// <returns></returns>
  public async Task<Basket?> GetBasketAsync(string buyerId)
  {
    try { return await _basketCacheSrv.GetBasketAsync(buyerId); }
    catch (RedisConnectionException) { return await _basketDBSrv.GetBasketAsync(buyerId); }

  }

  /// <summary>
  /// mc, Create basket on redis cache or db if redis throws error.
  /// </summary>
  /// <param name="buyerId"></param>
  /// <returns></returns>
  public async Task CreateBasketAsync(string buyerId)
  {
    try {  await _basketCacheSrv.CreateBasketAsync(buyerId); }
    catch (RedisConnectionException) {  await _basketDBSrv.CreateBasketAsync(buyerId); }
  }

  /// <summary>
  /// mc, Set basket on redis cache or on DB if cache throws error.
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
    catch (RedisConnectionException)
    {
      await _basketDBSrv.SetBasketItemAsync(buyerId, productId, qt);
    }
  }


  public async Task<int> GetBasketCountAsync(string buyerId)
  {
    try { return await _basketCacheSrv.GetBasketCountAsync(buyerId); }
    catch (RedisConnectionException) { return await _basketDBSrv.GetBasketCountAsync(buyerId); }
  }

  public async Task RemoveBasketAsync(string buyerId)
  {
    try { await _basketCacheSrv.RemoveBasketAsync(buyerId); }
    catch (RedisConnectionException) { await _basketDBSrv.RemoveBasketAsync(buyerId); }
  }

  public async Task RemoveBasketItemAsync(string buyerId, int productId)
  {
    try { await _basketCacheSrv.RemoveBasketItemAsync(buyerId, productId); }
    catch (RedisConnectionException) { await _basketDBSrv.RemoveBasketItemAsync(buyerId, productId); }
  }

  
}
