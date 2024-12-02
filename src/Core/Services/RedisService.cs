using Estore.Core.Entities.BasketAggregate;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Estore.Core.Services;
public class RedisService
{


  private readonly IDistributedCache _redisCache;
  private readonly int _cacheDurationHours = 5;
  public RedisService(IDistributedCache cache)
  {
    _redisCache = cache;
  }


  public async Task<T?> GetCachedDataAsync<T>(string key)
  {
    var jsonData = await _redisCache.GetStringAsync(key);

    if (jsonData == null)
      return default(T);

    return JsonSerializer.Deserialize<T>(jsonData);
  }

  public async Task SetCacheDataAsync<T>(string key, T data, TimeSpan? cacheDuration)
  {
    var options = new DistributedCacheEntryOptions
    {
      AbsoluteExpirationRelativeToNow = cacheDuration ?? TimeSpan.FromHours(_cacheDurationHours)
      //SlidingExpiration = cacheDuration
    };
    
    var jsonData = JsonSerializer.Serialize(data);
    await _redisCache.SetStringAsync(key, jsonData, options);
  }

  public async Task RemoveCachedDataAsync(string key)
  {
    
    await _redisCache.RemoveAsync(key);
  }

  public T? GetCachedData<T>(string key) { 
    var jsonData= _redisCache.GetString(key);
    if (jsonData == null)
      return default;
    return JsonSerializer.Deserialize<T>(jsonData);
  }

  public void SetCacheData<T>(string key,T data)
  {
    var jsonData= JsonSerializer.Serialize(data);
    _redisCache.SetString(key, jsonData);
  }


}
