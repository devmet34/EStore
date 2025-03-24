using EStore.App.Services;
using EStore.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
  public class BasketCacheServiceTest
  {
    [Fact]
    public async Task Test()
    {
      using var scope = Helper4Tests.GetServiceScope();

      var basketCacheSrv = scope.ServiceProvider.GetRequiredService<IBasketCacheService>();
      var t = await basketCacheSrv.GetBasketCountAsync("sdsdsd");
      int g = 1;
    }



  }
}
