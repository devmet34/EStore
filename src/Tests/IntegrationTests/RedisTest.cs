using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using NuGet.Protocol;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc.Testing;
using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Interfaces;
using Assert = Xunit.Assert;
using EStore.App.Services;

namespace IntegrationTests;
public class RedisTest
{
  private WebApplicationFactory<Program> app= ProgramFactory.webApplicationFactory;
  private  IServiceScope scope;
  private  RedisService redisService;
  ITestOutputHelper output;

  public RedisTest(ITestOutputHelper output) {
    this.output = output;
    scope= app.Services.CreateScope();
    redisService=scope.ServiceProvider.GetRequiredService<RedisService>();
  }

  [Fact]
  public async Task Test()
  {
    
    

    //var redisCache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
    //redisCache.SetString("key1", "value1");
    //redisCache.SetString("key2", "value2");


    //var redisMux = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
    //var redis=redisMux.GetDatabase();

    string str = "test";
    var testDto = new TestDto(12, "myname");    

    //await redis.SetCachedDataAsync<string>("str", str, TimeSpan.FromSeconds(100));
    //await redis.SetCachedDataAsync<TestDto>("dto1",testDto, TimeSpan.FromSeconds(100));
   
    var strCache = await redisService.GetCachedDataAsync<string>("str");
    var testDtoCache = await redisService.GetCachedDataAsync<TestDto>("dto1");

   //output.WriteLine(strCache + "/" +testDtoCache.ToJson()); 
    
     
  

  }

  [Fact]
  public async Task BasketTests()
  {
    string buyerId = "myBuyerID";  
    
    var basketService = scope.ServiceProvider.GetRequiredService<IBasketService>();

    while (true)
    {
      output.WriteLine("starting");
      await basketService.CreateBasketAsync(buyerId);
      
      Thread.Sleep(1000);
      await basketService.CreateBasketAsync(buyerId);
      var basketFromRedis = await basketService.GetBasketAsync(buyerId);
      Assert.True(basketFromRedis?.BasketItems.Count == 2);
      await basketService.RemoveBasketItemAsync(buyerId, 1);
      basketFromRedis = await basketService.GetBasketAsync(buyerId);
      Assert.True(basketFromRedis?.BasketItems.Count == 1);
      await basketService.RemoveBasketAsync(buyerId);
      int t = 1;
    }

  }

  public class TestDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsOk { get; set; }=true;
    public List<string> MyList { get; set; } = new List<string>() { "str1", "str2", "str3" };

    public TestDto(int id,string name) {
      Id = id;
      Name = name;
    }

    
  }

}//eo class
