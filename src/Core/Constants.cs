using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core;
public class Constants
{
  public const int pageSize = 20;
  public const string productsCacheKey = ":Products";
  public const string basketCacheKey = ":Basket";
  public const string basketCountCacheKey = ":BasketCount";
  public const char basketCacheDelimeter = '_';
  public readonly static TimeSpan basketCacheDuration= TimeSpan.FromSeconds(180000);

  public const string redisGenericException = "Redis cache error, check logs for details. ";
  public const string redisSetErrorMsg = "Error during setting redis cache data. ";
  public const string redisGetErrorMsg = "Error during getting redis cache data. ";
  public const string redisRemoveErrorMsg = "Error during removing data. ";
}
