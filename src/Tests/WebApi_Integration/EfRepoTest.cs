using Estore.Core.Entities;
using Estore.Core.Interfaces;
using Estore.Core.Services;
using EStore.Core.Query;
using EStore.Infra.EF;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace WebApi_Integration;
public class EfRepoTest
{
  ITestOutputHelper output;
  IRepo<Basket> repo;
  string buyerId = "fefefd7e-d506-45ad-aa9d-7dc80cd15dc1";
  public EfRepoTest(ITestOutputHelper output)
  {
    this.output = output;
   
  }

  

  [Fact]
  public async void Test()
  {
    IRepo<Basket> repo;
    var app = ProgramFactory.webApplicationFactory;
    using (var scope = app.Services.CreateScope())
    {
      repo = scope.ServiceProvider.GetRequiredService<IRepo<Basket>>();
      var q = new BasketQuery(buyerId).GetQuery();
      var r = await repo.GetByQuery(q);
      output.WriteLine(r.ToJson());
      return;
      var basketService=scope.ServiceProvider.GetRequiredService<BasketService>();

      


      var a= await basketService.GetBasketAsync(buyerId);
      var b=await basketService.GetBasketAsync(buyerId,true);
      output.WriteLine(a.ToJson());
      output.WriteLine(b.ToJson());
      output.WriteLine(b.BasketItems.ToJson());
      return;
      repo =scope.ServiceProvider.GetRequiredService<IRepo<Basket>>();
      var x = repo.Query().Where(b => b.BuyerId == buyerId).Include(b => b.BasketItems);
      //x =x.Where(b => b.BuyerId == buyerId).Include(b => b.BasketItems);
      var t = await repo.GetByQuery(x);

      

      
    }
    

  }
}
