using Microsoft.CodeAnalysis.CSharp.Syntax;
using Serilog;
using StackExchange.Redis;

namespace EStore.Web.Config;


public class ConfigRedis
{
  private readonly static int connectTimeout = 2000;
  private static string? connectionString=null;
  public static void AddRedis(WebApplicationBuilder builder)
  {
     connectionString = builder.Configuration["Redis:ConnectionString"] ?? throw new InvalidOperationException("Connection string 'redis' not found.");
    //Log.Debug("conn:"+connectionString);
    
    builder.Services.AddStackExchangeRedisCache(options =>
    {
      var config = new ConfigurationOptions();
      config= ConfigurationOptions.Parse(connectionString);
      config.ConnectTimeout=connectTimeout;
      config.SyncTimeout=connectTimeout;
      options.ConfigurationOptions = config;
      //options.Configuration = connectionString;
      options.InstanceName = "Estore";
      
      
      
    });
    
    
    /*
    builder.Services.AddSingleton<IConnectionMultiplexer>(
      ConnectionMultiplexer.Connect( connectionString, options =>
      {
        options.SyncTimeout = 2000;
        options.ConnectTimeout = 2000;
      }
      
      ));
     */

  }

  /*
  static IConnectionMultiplexer? ConnectToRedis(ILogger<ConfigRedis> logger)
  {
    try
    {
      return ConnectionMultiplexer.Connect(connectionString, options =>
      {
        options.SyncTimeout = 2000;
        options.ConnectTimeout = 2000;
      });
      //redis = services.GetRequiredService<IConnectionMultiplexer>();
    }
    catch (Exception ex) {
      logger.LogCritical("Redis error: "+ex.ToString());
      return null;  }

  }

  public static bool isRedisConnected= false;  
  public static void HealthCheck(IServiceProvider services)
  {
    var logger = services.GetRequiredService<ILogger<ConfigRedis>>();
    IConnectionMultiplexer? redis = ConnectToRedis(logger);

    if (redis == null)
      return;



    Task.Run(
      () =>
      {
        while (true) {

          if (!redis.IsConnected)
          {
            isRedisConnected = false;
            logger.LogCritical("Redis not connected");
          }
          else { isRedisConnected = true; }
          
          Thread.Sleep(20000);
        }
        
        
      }
      );
  }
  */
}//eo class
