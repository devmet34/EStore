using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EStore.Infra
{
  public static class Dependencies
  {
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
      bool useInMemoryDatabase = false;

      //if (configuration.get)

    }
  }
}
