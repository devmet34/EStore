using BenchmarkDotNet.Attributes;
using Estore.Core.Entities;
using Estore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_Integration.BenchmarkDotNet;

//mc; benchmark.net needs a class with test method to run.  
public class BenchEfRepo
{
  
    IRepo<Product> repo;
    public static IQueryable<Product>? query { get; set; }
    public WebApplicationFactory<Program>? app;

    [GlobalSetup]
    public void GlobalSetup()
    {

      app = ProgramFactory.webApplicationFactory;


    }
    //public BenchmarkClass(IQueryable<Product> query) {  this.query = query;}
    [Benchmark]
    [IterationCount(10)]
  public async Task Test()
  {

    using var scope = app.Services.CreateScope();
    repo = scope.ServiceProvider.GetRequiredService<IRepo<Product>>();

    query = repo.Query();
    query = query.Where(p => p.CategoryId > 1).AsNoTracking();
    var t = await repo.ListByQueryAsync(query); 
    
    //await repo.ListByQueryAsync(query);
  }

  /*
  public void Test()
  {

    using var scope = app.Services.CreateScope();
    repo = scope.ServiceProvider.GetRequiredService<IRepo<Product>>();

    query = repo.Query();
    query = query.Where(p => p.CategoryId > 1);
    var t = query.AsNoTracking().ToList();
    //await repo.ListByQueryAsync(query);
  }
  */

}//eo class
