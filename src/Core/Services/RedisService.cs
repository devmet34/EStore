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
      return default;

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

  public void RemoveCachedData(string key)
  {
    _redisCache.Remove(key);
  }




}
