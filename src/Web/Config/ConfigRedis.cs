using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Serilog;
using StackExchange.Redis;
using Web;

namespace EStore.Web.Config;


public static class ConfigRedis
{
  //private readonly static int connectTimeout = Constants.connectTimeoutMs;
  private static string? connectionString=null;
  public static void AddRedis(WebApplicationBuilder builder)
  {
     connectionString = builder.Configuration["Redis:ConnectionString"] ?? throw new InvalidOperationException("Connection string 'redis' not found.");
    var timeout = builder.Configuration.GetValue<int>("redis:timeoutMs");   

    builder.Services.AddStackExchangeRedisCache(options =>
    {
      var config = new ConfigurationOptions();            
      config= ConfigurationOptions.Parse(connectionString);
      //config.AbortOnConnectFail = false;
      config.SyncTimeout = timeout; //mc, this timeout value also applies to other timeouts automatically by redis impl.
      //config.ConnectTimeout=connectTimeout;      
      options.ConfigurationOptions = config;      
      options.InstanceName = "Estore";

    });

    

  }

  /// <summary>
  /// mc, Post config rediscache options. This is supposed to be called after app = builder.Build(); mainly because some options/configs in redis like logging is designed to get service instance, example iloggerfactory. 
  ///
  /// 
  /// A separate dedicated logger for redis would be better choice or even better; log only critical errors on app and get rest logged on redis server. Needed to troubleshoot connection errors quickly hence this, need to check if redis already have loggers on the server.   
  /// </summary>
  /// <param name="sp"></param>
  public static void PostConfigRedisCacheOptions (IServiceProvider sp)
  {
    var opt = sp.GetService<Microsoft.Extensions.Options.IOptions<Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions>>();    
    var loggerFactory = sp.GetService<ILoggerFactory>();
    if (opt!=null && loggerFactory!=null)
      opt!.Value.ConfigurationOptions!.LoggerFactory = loggerFactory;
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
