using Estore.Core.Interfaces;
using Estore.Core.Services;
using EStore.Infra.EF;

namespace EStore.Web.Config;

public static class ConfigureCoreServices
{
  public static IServiceCollection AddCoreServices(this IServiceCollection services)
  {
    services.AddScoped(typeof(IRepo<>),typeof( EfRepo<>));
    services.AddScoped<IBasketService, BasketCacheService>();
    services.AddScoped<ProductService>();
    services.AddScoped<OrderService>();
    services.AddScoped<FilterService>();
    services.AddScoped<RedisService>();
    services.AddScoped<IRepoOrder, RepoOrder>();
    return services;

  }
}
