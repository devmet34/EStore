using EStore.App.Services;
using EStore.Core.Interfaces;
using EStore.Infra.EF.Repos;

namespace EStore.Web.Config;

public static class ConfigureCoreServices
{
  public static IServiceCollection AddCoreServices(this IServiceCollection services)
  {
    services.AddScoped(typeof(IRepo<>), typeof(GenericRepo<>));
    services.AddScoped(typeof(IRepoRead<>), typeof(GenericReadRepo<>));
    services.AddScoped<IRepoOrder, OrderRepo>();
    services.AddScoped<IBasketCacheService, BasketCacheService>();
    services.AddScoped<IBasketDBService, BasketDBService>();
    services.AddScoped<IBasketService, BasketService>();
    services.AddScoped<ProductService>();
    services.AddScoped<OrderService>();
    services.AddScoped<FilterService>();
    services.AddScoped<RedisService>();

    //services.AddScoped<IBasketService>( BasketServiceFactory);


    return services;

  }


  private class ConfigureCoreServices_ { } //mc, this is for TCategory for ilogger<T>
}//eo class
