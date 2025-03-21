﻿using EStore.App.Services;
using EStore.Core.Entities.BasketAggregate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationTests;
using EStore.Core.Interfaces;
using Xunit;

namespace IntegrationTests;
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
