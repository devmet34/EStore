using EStore.App.Services;
using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace IntegrationTests;
public class OrderTest
{

  [Fact]
  public async void Test()
  {
    
    var app = ProgramFactory.webApplicationFactory;    
    using var scope = app.Services.CreateScope();

    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var userId = config["userId"];//mc; getting config from secret or appsettings.json
    //var basketService=scope.ServiceProvider.GetRequiredService<BasketService>();
    
    var productService= scope.ServiceProvider.GetRequiredService<ProductService>();
    var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
    var basketService= scope.ServiceProvider.GetRequiredService<IBasketService>();
    
    var product = await productService.GetProductAsync(1);
    var basket = await basketService.GetOrCreateBasketAsync(userId!);
    basket?.SetBasketItem(product!.Id, 2, product.Price);
    //var basket= await basketService.GetBasketAsync(userId);
    await orderService.CreateOrderAsync(userId!);

    var order = await orderService.GetAllOrdersAsync(userId!);
    //Xunit.Assert.True(order.FirstOrDefault().OrderItems.Count > 1);
  }
}
