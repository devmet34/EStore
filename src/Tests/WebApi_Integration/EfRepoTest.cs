using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Estore.Core.Entities;
using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Interfaces;
using Estore.Core.Services;
using EStore.Core.Query;
using EStore.Infra.EF;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Writers;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Integration.BenchmarkDotNet;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace WebApi_Integration;
public class EfRepoTest
{
  ITestOutputHelper output;
  IRepo<Basket> repo;
  IRepo<Product> repoProd;
  string buyerId = "fefefd7e-d506-45ad-aa9d-7dc80cd15dc1";
  public EfRepoTest(ITestOutputHelper output)
  {
    this.output = output;
   
  }

  

  [Fact]
  public async void Test()
  {
    //IRepo<Basket> repo;
    //var app = ProgramFactory.webApplicationFactory;
    
    var config = Config.GetConfig();
    var res = BenchmarkRunner.Run<BenchEfRepo>(config);
    return;
    /*
    using (var scope = app.Services.CreateScope())
    {
      repo = scope.ServiceProvider.GetRequiredService<IRepo<Basket>>();
      repoProd=scope.ServiceProvider.GetRequiredService<IRepo<Product>>();
      TestProductRepo();
      return;
      var query = repoProd.Query();
      query = query.Where(p => p.CategoryId > 1);
       
      //var res=BenchmarkRunner.Run<BenchmarkClass>(config);
      output.WriteLine(res.Table.ToString());
      //output.WriteLine(res.ToJson());
      
      int ii = 2;
      return;
      await TestProductRepo();
      return;
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
    
    */
  }

  private async Task TestProductRepo()
  {
  
    int waitMs = 100;
    int batch = 3;
    var query=repoProd.Query();
    query=query.Where(p => p.CategoryId > 1);
    var res = query.AsNoTracking().ToList();
    return;


    for (int i=1; i<=batch; i++)
    {

      //var res = query.AsNoTracking().ToList();
      //output.WriteLine(res?.Count().ToString());
      //Thread.Sleep(waitMs);
    }




  }

}//eo class

public class BenchmarkClass
{
  IRepo<Product> repo;
  public static IQueryable<Product> query { get; set;}
  public WebApplicationFactory<Program> app;

  [GlobalSetup]
  public void GlobalSetup() {
    
    app = ProgramFactory.webApplicationFactory;
    
    
  }
  //public BenchmarkClass(IQueryable<Product> query) {  this.query = query;}
  [Benchmark]
  [IterationCount(2)]
  
  public async Task test()
  {
    
    using var scope = app.Services.CreateScope();
    repo = scope.ServiceProvider.GetRequiredService<IRepo<Product>>();

    query = repo.Query();
    query = query.Where(p => p.CategoryId > 1);
    var t=query.AsNoTracking().ToList();
    //await repo.ListByQueryAsync(query);
  }

}
  
  
 
