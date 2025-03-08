using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Estore.Core.Services;
public class RedisService
{

  private readonly ILogger<RedisService> _logger;
  private readonly IDistributedCache _redisCache;
  private readonly TimeSpan _defaultCacheDuration = TimeSpan.FromHours(5);
  public bool IsRedisBroken { get; private set; }
  public RedisService(IDistributedCache cache, ILogger<RedisService> logger)
  {
    _redisCache = cache;
    _logger = logger;
  }

  /// <summary>
  /// mc, If flag is true in same scope/request then throw. IsRedisBroken is set to true in this class for every scoped request when redis library throws error.  
  /// </summary>
  /// <exception cref="RedisGenericException"></exception>
  private void ThrowIfRedisBroken()
  {
    if (IsRedisBroken)
      throw new RedisGenericException();
  }



  /// <summary>
  /// mc, Get cached data with given key from redis then deserialize and return given T, throws error.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="key"></param>
  /// <returns></returns>
  public async Task<T?> GetCachedDataAsync<T>(string key)
  {

    ThrowIfRedisBroken();

    try
    {
      var jsonData = await _redisCache.GetStringAsync(key);

      if (jsonData == null)
        return default;

      return JsonSerializer.Deserialize<T>(jsonData);
    }
    catch (Exception ex) {
      _logger.LogError(Constants.redisGetErrorMsg+ex.ToString());
      IsRedisBroken=true;
      throw;
    }
  }
  /// <summary>
  /// mc, set/save given T data with given key to redis after serializing it to json, throws error.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="key"></param>
  /// <param name="data"></param>
  /// <param name="cacheDuration"></param>
  /// <returns></returns>
  public async Task SetCacheDataAsync<T>(string key, T data, TimeSpan? cacheDuration)
  {
    ThrowIfRedisBroken();

    try
    {
      var options = new DistributedCacheEntryOptions
      {
        AbsoluteExpirationRelativeToNow = cacheDuration ?? _defaultCacheDuration
        //SlidingExpiration = cacheDuration
      };

      var jsonData = JsonSerializer.Serialize(data);
      await _redisCache.SetStringAsync(key, jsonData, options);
    }
    catch(Exception ex)
    {
      _logger.LogError(Constants.redisSetErrorMsg+ex.ToString());
      IsRedisBroken = true;
      throw;
    }
  }

  /// <summary>
  /// Remove cached data with given key, throws error. 
  /// </summary>
  /// <param name="key"></param>
  /// <returns></returns>
  public async Task RemoveCachedDataAsync(string key)
  {
    ThrowIfRedisBroken();
    try
    {
      await _redisCache.RemoveAsync(key);
    }
    catch(Exception ex)
    {
      IsRedisBroken = true;
      _logger.LogError(Constants.redisRemoveErrorMsg+ex.ToString());
      throw;
    }
  }

  //************* Sync Methods
  public T? GetCachedData<T>(string key)
  {
    try
    {

      var jsonData = _redisCache.GetString(key);
      if (jsonData == null)
        return default;
      return JsonSerializer.Deserialize<T>(jsonData);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex.ToString());
      return default;
    }
  }

  public void SetCacheData<T>(string key,T data)
  {
    var jsonData= JsonSerializer.Serialize(data);
    _redisCache.SetString(key, jsonData);
  }


}
