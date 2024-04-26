using Estore.Core.Interfaces;
using Estore.Core.Services;
using EStore.Infra.EF;

namespace EStore.Web.Config;

public static class ConfigureCoreServices
{
  public static IServiceCollection AddCoreServices(this IServiceCollection services)
  {
    services.AddScoped(typeof(IRepo<>),typeof( EfRepo<>));
    services.AddScoped<BasketService>();
    services.AddScoped<ProductService>();
    return services;

  }
}
