using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Integration;
using Xunit;

namespace WebApi_IntegrationTests;
public class BasketTest
{

  [Fact]
  public async void Test()
  {

    var app = ProgramFactory.webApplicationFactory;
    using var scope = app.Services.CreateScope();

    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var userId = config["userId"];//mc; getting config from secret or appsettings.json
                                  //var basketService=scope.ServiceProvider.GetRequiredService<BasketService>();

    var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
    var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
    var basketService = scope.ServiceProvider.GetRequiredService<BasketDBService>();

    var basket = await basketService.GetOrCreateBasketAsync(userId);
    //await basketService.SetBasketItemAsync(userId, 1, 2);

 
    var date=DateTime.Parse("03/12/2024").Date;
    var q = basketService.Repo.Query();
    var t = q.Where(b => b.BasketCreatedAt.Date == date).FirstOrDefault();
    int c = 1;
    /*
    var product = await productService.GetProductAsync(1);
    var basket = new Basket(userId);
    basket.SetBasketItem(product.Id, 2, product.Price);
    */
  }
}
