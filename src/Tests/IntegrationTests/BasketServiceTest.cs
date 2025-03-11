using Estore.Core.Interfaces;
using Estore.App.Services;
using EStore.Infra.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests;
public class BasketServiceTest:ProgramFactory
{
  ITestOutputHelper output;
  
  public BasketServiceTest(ITestOutputHelper testOutput) 
  { 
    output = testOutput;
    
  }

  [Fact]
  public async void Test()
  {
    output.WriteLine("test");
    var q = SpecEvaluator.Query();
    q.Where(b => b.Id == 1).Include(b => b.BasketItems).ThenInclude(b => b.Product);
   
    var app = ProgramFactory.webApplicationFactory;
    using (var scope = app.Services.CreateScope())
    {
      var basketService = scope.ServiceProvider.GetRequiredService<IBasketService>();
      var client = app.CreateClient();
      var url = "home/AddProductAsync";
      
      
    }
  }
}
