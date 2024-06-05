using Microsoft.AspNetCore.Connections.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using EStore.Web.Models;


namespace UnitTests;
public class JsonRedisTest
{
  string redisConnString = "localhost:6379";

  [Fact]
  public async Task Test()
  {
    var mux = ConnectionMultiplexer.Connect(redisConnString, options =>
    {
      
    });
    var redisDb=mux.GetDatabase();
    var json =  redisDb.HashGet("Estore:Products","data");
    var res= JsonSerializer.Deserialize<IEnumerable<ProductVM>>(json);
    int a = 1;
  }
}
