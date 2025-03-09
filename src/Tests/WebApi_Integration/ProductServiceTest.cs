using Estore.App.Services;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace WebApi_Integration;
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
    var productService=scope.ServiceProvider.GetRequiredService<ProductService>();
    var res = await productService.FilterProductsAsync(new Estore.Core.Models.FilterModel() { MainCat="balls" } );
    var res2 = await productService.FilterProductsAsync(new Estore.Core.Models.FilterModel() { MainCat = "balls",PriceMax=2 });
    var res3= await productService.FilterProductsAsync(new Estore.Core.Models.FilterModel() { SubCat = "basketball" });
    var res4= await productService.FilterProductsAsync(new Estore.Core.Models.FilterModel() { PriceMin=2,PriceMax=10 });

   

    foreach (var item in res)
      output.WriteLine(item.ToJson());

    output.WriteLine("-----------------");

    foreach (var item in res2)
      output.WriteLine(item.ToJson());
    output.WriteLine("-----------------");

    foreach (var item in res3)
      output.WriteLine(item.ToJson());

    int t = 1;
    
    
  }
}
