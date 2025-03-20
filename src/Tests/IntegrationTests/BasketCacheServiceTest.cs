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
      var app = ProgramFactory.webApplicationFactory;
      using var scope = app.Services.CreateScope();

      var basketCacheSrv = scope.ServiceProvider.GetRequiredService<IBasketCacheService>();
      var t = await basketCacheSrv.GetBasketCountAsync("sdsdsd");
      int g = 1;
    }



  }
}
