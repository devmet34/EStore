using Estore.Core.Interfaces;
using Estore.Core.Services;
using EStore.Infra.EF.Repos;

namespace EStore.Web.Config;

public static class ConfigureCoreServices
{
  public static IServiceCollection AddCoreServices(this IServiceCollection services)
  {
    services.AddScoped(typeof(IRepo<>),typeof( GenericRepo<>));
    services.AddScoped<IRepoRead, GenericReadRepo>();
    services.AddScoped<IRepoOrder, OrderRepo>();
    services.AddScoped<IBasketService, BasketCacheService>();
    services.AddScoped<BasketService>(); //mc for tests
    services.AddScoped<ProductService>();
    services.AddScoped<OrderService>();
    services.AddScoped<FilterService>();
    services.AddScoped<RedisService>();
    
    return services;

  }
}
