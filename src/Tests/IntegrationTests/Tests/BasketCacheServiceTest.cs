using EStore.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Assert = Xunit.Assert;

namespace IntegrationTests.Tests
{
    public class BasketCacheServiceTest
    {
        [Fact]
        public async Task Test()
        {
            using var scope = Helper4Tests.GetServiceScope();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var userId = config["userId"] ?? throw new ArgumentException();//mc; getting config from secret or appsettings.json
            var basketCacheSrv = scope.ServiceProvider.GetRequiredService<IBasketCacheService>();

            var basket = await basketCacheSrv.GetBasketAsync(userId);
            if (basket is not null)
                await basketCacheSrv.RemoveBasketAsync(userId);
            await basketCacheSrv.CreateBasketAsync(userId);
            await basketCacheSrv.SetBasketItemAsync(userId, 1, 2);
            basket = await basketCacheSrv.GetBasketAsync(userId);

            Assert.NotNull(basket);
            Assert.True(basket.BasketItems.Any());

            await basketCacheSrv.RemoveBasketAsync(userId);

        }



    }
}
