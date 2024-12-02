using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Services;
public class BasketCacheService: Interfaces.IBasketService
{
  private readonly IRepo<Basket> _repo;
  private readonly ILogger<IBasketService> _logger;
  private readonly ProductService _productService;
  private readonly RedisService _redisService;
  private readonly TimeSpan cacheDuration= Constants.basketCacheDuration;

  public BasketCacheService(IRepo<Basket> repo, ILogger<IBasketService> logger, ProductService productService, RedisService redisService)
  {
    _repo = repo;
    _logger = logger;
    _productService = productService;
    _redisService = redisService;
  }

  private string GetBasketCacheKey(string buyerId) { 
    return Constants.basketCacheKey + Constants.basketCacheDelimeter+ buyerId;
  }

  public async Task<Basket?> GetOrCreateBasketAsync(string buyerId)
  {
    buyerId.GuardNullOrEmpty();
    string cacheKey = GetBasketCacheKey(buyerId);
    var basket = await GetBasketWithKeyAsync(cacheKey);
    if (basket != null)
    {
      _logger.LogDebug("basket already exist");
      return basket;
    }
    _logger.LogInformation("creating basket"); 
    basket = new(buyerId);
    await _redisService.SetCacheDataAsync(cacheKey, basket, cacheDuration );
    return await GetBasketAsync(cacheKey);
  }

  public async Task SetBasketItemAsync(string buyerId, int productId, int qt)
  {
    string cacheKey = GetBasketCacheKey(buyerId);
    var basket = await GetBasketWithKeyAsync(cacheKey);
    basket.GuardNull();

    var product = await _productService.GetProductForBasketAsync(productId);
    product.GuardNull();

    basket!.SetBasketItem(productId, qt, product!.Price,product);
    await _redisService.SetCacheDataAsync(cacheKey, basket, cacheDuration);


  }

  public async Task<Basket?> GetBasketAsync(string buyerId)
  {
    var cacheKey= GetBasketCacheKey(buyerId);
    return await _redisService.GetCachedDataAsync<Basket>(cacheKey);
  }

  private async Task<Basket?> GetBasketWithKeyAsync(string cacheKey)
  {
    
    return await _redisService.GetCachedDataAsync<Basket>(cacheKey);
  }

  public Basket? GetBasket(string cacheKey)
  {
    return _redisService.GetCachedData<Basket>(cacheKey);
  } 

  public Task<Basket?> GetBasketAsync(string buyerId, bool includeBasketItems = true, bool includeAll = false)
  {
    throw new NotImplementedException();
  }

  public async Task RemoveBasketItemAsync(string buyerId, int productId)
  {
    var key=GetBasketCacheKey(buyerId);
    var basket = await _redisService.GetCachedDataAsync<Basket>(key);
    basket.GuardNull();

    basket!.RemoveBasketItem(productId);
    
    await _redisService.SetCacheDataAsync(key, basket, cacheDuration);




  }

  public async Task RemoveBasketAsync(Basket basket)
  {
    basket.GuardNull();
    var cacheKey= GetBasketCacheKey(basket.BuyerId);
    await _redisService.RemoveCachedDataAsync(cacheKey);
  }
}//
