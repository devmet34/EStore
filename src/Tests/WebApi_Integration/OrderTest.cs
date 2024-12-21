using Estore.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace WebApi_Integration;
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
    var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
    
    //var basket= await basketService.GetBasketAsync(userId);
    await orderService.CreateOrderAsync(userId);

    var order = await orderService.GetAllOrdersAsync(userId);
    //Xunit.Assert.True(order.FirstOrDefault().OrderItems.Count > 1);
  }
}
