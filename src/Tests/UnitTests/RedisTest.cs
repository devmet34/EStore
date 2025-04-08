using EStore.Core.Models;
using StackExchange.Redis;
using StackExchange.Redis.KeyspaceIsolation;
using System.Text.Json;


namespace UnitTests;
public class RedisTest
{
  string redisConnString = "localhost:6379";

  [Fact]
  public async Task Test()
  {
    ConfigurationOptions options = new ConfigurationOptions();

    var mux = ConnectionMultiplexer.Connect(redisConnString, options =>
    {
      options.AbortOnConnectFail = false;

    });
    var key = "Estore:Test";
    var redisDb = mux.GetDatabase().WithKeyPrefix(new RedisKey("estore"));

    var json = redisDb.HashGet("Estore:Products", "data");
    var res = JsonSerializer.Deserialize<IEnumerable<ProductVM>>(json);
    int a = 1;
  }
}
