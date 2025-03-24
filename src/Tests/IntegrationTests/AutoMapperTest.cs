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
      //var basket = await basketRepo.Query.Where(b => b.BuyerId == buyerId).Include(b => b.BasketItems).ThenInclude(bi => bi.Product).FirstOrDefaultAsync();
      var basketVM= await basketRepo.Query.Where(b => b.BuyerId == buyerId).Include(b => b.BasketItems).ProjectTo<BasketVM>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
      int g = 1;
    }


  }
}
