using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EStore.Core;
using EStore.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using EStore.Core.Entities.BasketAggregate;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using EStore.Web.Models;
using Microsoft.Extensions.Configuration;
using EStore.Web.Config;
using AutoMapper;
using EStore.App.Services;
using EStore.Core.Entities;

namespace IntegrationTests
{
  public class AutoMapperTest
  {

    [Fact]

    public async Task Test()
    {
      var buyerId=Constants.userId;
      using var scope = Helper4Tests.GetServiceScope();
      var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();      
      var basketRepo = scope.ServiceProvider.GetRequiredService<IRepo<Basket>>();
      var redisService=scope.ServiceProvider.GetRequiredService<RedisService>();

      var basket=new Basket(buyerId);      
      var product = new Product("t", null, null, 2m, 5, null, "asdsdsd.sadsdsad");
      basket.SetBasketItem(1, 2, 3.50m,product);

      redisService.SetCacheData("b",basket);

      var b = redisService.GetCachedData<Basket>("b");
      var b2= redisService.GetCachedData<EStore.Core.Models.BasketVM>("b");


      var basketVM= await basketRepo.Query.Where(b => b.BuyerId == buyerId).Include(b => b.BasketItems).ThenInclude(bi=>bi.Product). ProjectTo<EStore.Core.Models.BasketVM>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
      int g = 1;
    }


  }
}
