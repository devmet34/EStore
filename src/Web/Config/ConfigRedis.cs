using Serilog;
using StackExchange.Redis;

namespace EStore.Web.Config;


public class ConfigRedis
{
  public static void AddRedis(WebApplicationBuilder builder)
  {
    var connectionString = builder.Configuration["Redis:ConnectionString"] ?? throw new InvalidOperationException("Connection string 'redis' not found.");
    //Log.Debug("conn:"+connectionString);
    
    builder.Services.AddStackExchangeRedisCache(options =>
    {
      
      options.Configuration = connectionString;
      options.InstanceName = "Estore";
    });
    
/*
    builder.Services.AddSingleton<IConnectionMultiplexer>(
      ConnectionMultiplexer.Connect( connectionString, options =>
      {
        options.SyncTimeout = 10000;
        options.ConnectTimeout = 10000;
      }
      
      ));
     
     */

  }
}
