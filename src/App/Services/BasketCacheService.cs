﻿using EStore.Core;
using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Exceptions;
using EStore.Core.Extensions;
using EStore.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.App.Services;
public class BasketCacheService : IBasketCacheService
{

  private readonly ILogger<BasketCacheService> _logger;
  private readonly ProductService _productService;
  private readonly RedisService _redisService;
  private readonly TimeSpan cacheDuration = Constants.basketCacheDuration;
  private readonly IConfiguration _config;

  public BasketCacheService(ILogger<BasketCacheService> logger, ProductService productService, RedisService redisService, IConfiguration config)
  {

    _logger = logger;
    _productService = productService;
    _redisService = redisService;
    _config = config;
    var cacheDurationHours = int.Parse(_config.GetSection("redis:basketCacheDurationHours").Value!);
    cacheDuration = TimeSpan.FromHours(cacheDurationHours);
  }


  private string GetBasketCacheKey(string buyerId)
  {
    return Constants.basketCacheKey + Constants.basketCacheDelimeter + buyerId;
  }

  private string GetBasketCountCacheKey(string buyerId)
  {
    return Constants.basketCountCacheKey+ Constants.basketCacheDelimeter + buyerId;
  }

  public async Task CreateBasketAsync(string buyerId)
  {

    buyerId.GuardNullOrEmpty();
    var hasBasketCreated=await GetBasketCountAsync(buyerId)>=0;

    if (hasBasketCreated)
    {
      _logger.LogDebug("Basket already exists in redis cache");
      return;
    }

    Basket? basket = null;
    //basket = await GetBasketAsync(buyerId);
    _logger.LogDebug("Creating basket in redis cache");
    basket = new(buyerId);
    string cacheKey = GetBasketCacheKey(buyerId);
    await _redisService.SetCacheDataAsync(cacheKey, basket, cacheDuration);
    await SetBasketCountAsync(buyerId, 0);
    
  }

  public async Task<int> GetBasketCountAsync(string buyerId)
  {
    buyerId.GuardNullOrEmpty();
    var cacheKey = GetBasketCountCacheKey(buyerId);
    var countStr= await _redisService.GetCachedDataAsync(cacheKey);    
    if (countStr != null)
      return int.Parse(countStr!);
    return -1;
  }

  public async Task<Basket?> GetBasketAsync(string buyerId)
  {
    var cacheKey = GetBasketCacheKey(buyerId);

    return await _redisService.GetCachedDataAsync<Basket>(cacheKey);

  }

  public async Task SetBasketItemAsync(string buyerId, int productId, int qt)
  {

    //string cacheKey = GetBasketCacheKey(buyerId);
    //var basket = await GetBasketWithKeyAsync(cacheKey);
    var basket = await GetBasketAsync(buyerId);
    basket.GuardNull();

    var product = await _productService.GetProductForBasketAsync(productId);
    product.GuardNull();

    basket!.SetBasketItem(productId, qt, product!.Price, product);
    await _redisService.SetCacheDataAsync(GetBasketCacheKey(buyerId), basket, cacheDuration);
    await SetBasketCountAsync(buyerId, basket.BasketItemCount);

  }

  public async Task SetBasketCountAsync(string buyerId, int count)
  {
    buyerId.GuardNullOrEmpty();
    await _redisService.SetCacheDataAsync(GetBasketCountCacheKey(buyerId), count.ToString(), cacheDuration);
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

    var key = GetBasketCacheKey(buyerId);
    var basket = await _redisService.GetCachedDataAsync<Basket>(key);
    basket.GuardNull();

    basket!.RemoveBasketItem(productId);

    await _redisService.SetCacheDataAsync(key, basket, cacheDuration);
    await _redisService.SetCacheDataAsync(GetBasketCountCacheKey(buyerId),basket.BasketItemCount, cacheDuration);

  }

  public async Task RemoveBasketAsync(string buyerId)
  {
    buyerId.GuardNull();
    var cacheKey = GetBasketCacheKey(buyerId);
    await _redisService.RemoveCachedDataAsync(cacheKey);
    await _redisService.RemoveCachedDataAsync(GetBasketCountCacheKey(buyerId));
  }

 
}//
