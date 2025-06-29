using EStore.App.Services;
using EStore.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Assert = Xunit.Assert;

namespace IntegrationTests.Tests;
public class BasketTests
{

    [Fact]
    public async Task Test()
    {

        using var scope = Helper4Tests.GetServiceScope();

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var userId = config["userId"] ?? throw new ArgumentException();//mc; getting config from secret or appsettings.json


        var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        //var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
        var basketService = scope.ServiceProvider.GetRequiredService<IBasketService>();

        var basket = await basketService.GetBasketAsync(userId);
        if (basket is not null)
            await basketService.RemoveBasketAsync(userId);
        await basketService.CreateBasketAsync(userId);
        await basketService.SetBasketItemAsync(userId, 1, 2);
        basket = await basketService.GetBasketAsync(userId);

        Assert.NotNull(basket);
        Assert.Equal(userId, basket!.BuyerId);
        Assert.True(basket.BasketItems.Any());

        await basketService.RemoveBasketAsync(userId);
    }
}
