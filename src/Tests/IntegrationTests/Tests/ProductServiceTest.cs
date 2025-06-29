using EStore.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace IntegrationTests.Tests;
public class ProductServiceTest
{
    ITestOutputHelper output;
    public ProductServiceTest(ITestOutputHelper output)
    {
        this.output = output;

    }

    [Fact]
    public async Task TestAsync()
    {
        var app = ProgramFactory.webApplicationFactory;

        using var scope = app.Services.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        var res = await productService.FilterProductsAsync(new EStore.Core.Models.FilterModel() { MainCat = "balls" });
        var res2 = await productService.FilterProductsAsync(new EStore.Core.Models.FilterModel() { PriceMin = 2, PriceMax = 10 });
        var res3 = await productService.GetProductAsync(1);

        Assert.NotNull(res);
        Assert.NotEmpty(res);

        Assert.NotNull(res2);
        Assert.NotEmpty(res2);
        Assert.False(res2.Any(p => p.Price < 2 || p.Price > 10));

        Assert.NotNull(res3);
        Assert.Equal(1, res3!.Id);


    }
}
