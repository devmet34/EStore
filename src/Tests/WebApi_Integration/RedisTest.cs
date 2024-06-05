using Estore.Core.Services;
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

namespace WebApi_Integration;
public class RedisTest
{
  ITestOutputHelper output;

  public RedisTest(ITestOutputHelper output) { this.output = output; }

  [Fact]
  public async void Test()
  {
    var app = ProgramFactory.webApplicationFactory;
    using var scope = app.Services.CreateScope();
    //var redisCache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
    //redisCache.SetString("key1", "value1");
    //redisCache.SetString("key2", "value2");
    
    var redisService = scope.ServiceProvider.GetRequiredService<RedisService>();
    //var redisMux = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
    //var redis=redisMux.GetDatabase();
    
    string str = "test";
    var testDto = new TestDto(12, "myname");    

    //await redis.SetCachedDataAsync<string>("str", str, TimeSpan.FromSeconds(100));
    //await redis.SetCachedDataAsync<TestDto>("dto1",testDto, TimeSpan.FromSeconds(100));
   
    var strCache = await redisService.GetCachedDataAsync<string>("str");
    var testDtoCache = await redisService.GetCachedDataAsync<TestDto>("dto1");

   output.WriteLine(strCache + "/" +testDtoCache.ToJson()); 
    
     
  

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
