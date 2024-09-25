using EStore.Infra.EF;
using EStore.Infra.EF.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;



namespace ConsoleUnitTests;

public class DbContextTest
{
  static string _connStr = Constants.EstoreDbConnectionString;
  DbContextOptionsBuilder<EStoreDbContext> _options = new DbContextOptionsBuilder<EStoreDbContext>().UseSqlServer(_connStr);

 
  public async Task Test()
  {
    var options = new DbContextOptionsBuilder<EStoreDbContext>().UseSqlServer(_connStr);

    using var context = new EStoreDbContext(options.Options);

    await UpdateProducts.UpdateProductsBatch(context);
    return;

    var prods =  context.Products.AsNoTracking().Include(p => p.Category).Where(p => p.Category.MainCat.StartsWith("tennis shoes")).ToList();

    var prodsByName=  context.Products.AsNoTracking().Include(p => p.Category).Where(p=>p.Name.Contains("racket2")).ToList();

    var enumerable = context.Products.AsNoTracking().Include(p => p.Category).Where(p => p.Category.MainCat.StartsWith("tennis shoes")).AsEnumerable();
    var t = enumerable.ToList();
    foreach (var e in enumerable)
      e.ToString();

  

    

    int g = 1;
  }


}//eo cls
