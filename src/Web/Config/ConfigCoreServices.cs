using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Interfaces;
using Estore.Core.Services;
using EStore.Infra.EF.Repos;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EStore.Web.Config;

public static class ConfigureCoreServices
{
  public static IServiceCollection AddCoreServices(this IServiceCollection services)
  {
    services.AddScoped(typeof(IRepo<>),typeof( GenericRepo<>));
    services.AddScoped(typeof(IRepoRead<>), typeof(GenericReadRepo<>));
    services.AddScoped<IRepoOrder, OrderRepo>();
    services.AddScoped<IBasketCacheService, BasketCacheService>();
    services.AddScoped<IBasketDBService, BasketDBService>();
    services.AddScoped<IBasketService, BasketService>();
    services.AddScoped<ProductService>();
    services.AddScoped<OrderService>();
    //services.AddScoped<FilterService>();
    services.AddScoped<RedisService>();
    
    //services.AddScoped<IBasketService>( BasketServiceFactory);
    
    
    return services;

  }

  private static IBasketService BasketServiceFactory(IServiceProvider sp)
  {
    var repo = sp.GetRequiredService<IRepo<Basket>>();
    var productService = sp.GetRequiredService<ProductService>();
    var logger = sp.GetRequiredService<ILogger<ConfigureCoreServices_>>();    
    var redisService = sp.GetRequiredService<RedisService>();
    var key = ":Test";
      //var redisCache = sp.GetRequiredService<IDistributedCache>();
      if (!(redisService.GetCachedData<string>(key)).IsNullOrEmpty())
    {
      logger.LogInformation("******configcoreservices redis connected");
      return new BasketCacheService( sp.GetRequiredService<ILogger<BasketCacheService>>(), productService, redisService,sp.GetRequiredService<IConfiguration>());
    }
    logger.LogInformation("******configcoreservices redis not connected");
    return new BasketDBService(repo, sp.GetRequiredService<ILogger<BasketDBService>>(), productService);
  }
  private class ConfigureCoreServices_ { } //mc, this is for TCategory for ilogger<T>
}//eo class
