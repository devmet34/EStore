using EStore.App.Services;
using EStore.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests;
public class BasketTest
{

  [Fact]
  public async void Test()
  {

    using var scope = Helper4Tests.GetServiceScope();

    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var userId = config["userId"];//mc; getting config from secret or appsettings.json
                                  //var basketService=scope.ServiceProvider.GetRequiredService<BasketService>();

    var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
    var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
    var basketService = scope.ServiceProvider.GetRequiredService<IBasketService>();

    await basketService.CreateBasketAsync(userId);
    //await basketService.SetBasketItemAsync(userId, 1, 2);



    /*
    var product = await productService.GetProductAsync(1);
    var basket = new Basket(userId);
    basket.SetBasketItem(product.Id, 2, product.Price);
    */
  }
}
