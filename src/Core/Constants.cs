using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core;
public class Constants
{
  public const int pageSize = 20;
  public const string productsCacheKey = ":Products";
  public const string basketCacheKey = ":Basket";
  public const char basketCacheDelimeter = '_';
  public readonly static TimeSpan basketCacheDuration= TimeSpan.FromSeconds(180000);
}
