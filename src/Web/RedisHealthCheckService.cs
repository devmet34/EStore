
using EStore.Web.Config;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace EStore.Web;

public class RedisHealthCheckService : BackgroundService
{
  private readonly int connectTimeoutMs = 2000; 
  private readonly int healthCheckTimeoutMs = 60000;
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
      while (!stoppingToken.IsCancellationRequested)
      {
      IConnectionMultiplexer? redis = await ConnectToRedis();
      if (redis == null || !redis.IsConnected)
        {
          isRedisConnected = false;
          logger.LogCritical("Redis not connected");
        }
      else { isRedisConnected = true; }

       await Task.Delay(healthCheckTimeoutMs, stoppingToken);
        
      }
    //}

  }


  async Task<IConnectionMultiplexer?> ConnectToRedis()
  {
    var connectionString = configuration["Redis:ConnectionString"] ?? throw new InvalidOperationException("Connection string 'redis' not found.");
    try
    {
      return await ConnectionMultiplexer.ConnectAsync(connectionString, options =>
      {
        options.SyncTimeout = connectTimeoutMs;
        options.ConnectTimeout = connectTimeoutMs;
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
