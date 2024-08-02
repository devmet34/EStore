
using EStore.Web.Config;
using StackExchange.Redis;

namespace EStore.Web;

public class RedisHealthCheckService : BackgroundService
{
  private readonly int connectTimeout = 2000;
  private readonly int healthCheckTimeout = 20000;
  private string? connectionString = null;
  private readonly ILogger<RedisHealthCheckService> logger;
  private readonly IConfiguration configuration;
  private static bool isRedisConnected = false;


  public RedisHealthCheckService(ILogger<RedisHealthCheckService> logger, IConfiguration configuration)
  {
    this.logger = logger;
    this.configuration = configuration;
  }

  public static bool IsRedisConnected { get { return isRedisConnected; } }
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    IConnectionMultiplexer? redis = ConnectToRedis();

    if (redis != null)
    {
      while (!stoppingToken.IsCancellationRequested)
      {

        if (!redis.IsConnected)
        {
          isRedisConnected = false;
          logger.LogCritical("Redis not connected");
        }
        else { isRedisConnected = true; }

       await Task.Delay(healthCheckTimeout, stoppingToken);
      }
    }

  }


  IConnectionMultiplexer? ConnectToRedis()
  {
    var connectionString = configuration["Redis:ConnectionString"] ?? throw new InvalidOperationException("Connection string 'redis' not found.");
    try
    {
      return ConnectionMultiplexer.Connect(connectionString, options =>
      {
        options.SyncTimeout = 2000;
        options.ConnectTimeout = 2000;
      });
      //redis = services.GetRequiredService<IConnectionMultiplexer>();
    }
    catch (Exception ex)
    {
      logger.LogCritical("Redis error: " + ex.ToString());
      return null;
    }

  }
}
