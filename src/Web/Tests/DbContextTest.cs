using EStore.Infra.EF;
using Microsoft.EntityFrameworkCore;
using Web;


namespace EStore.Web.Tests;

public class DbContextTest
{
  static string _connStr = Constants.EstoreDbConnectionString;
  DbContextOptionsBuilder<EStoreDbContext> _options = new DbContextOptionsBuilder<EStoreDbContext>().UseSqlServer(_connStr);

 
  public async Task Test()
  {
    var options = new DbContextOptionsBuilder<EStoreDbContext>().UseSqlServer(_connStr);

    using var context = new EStoreDbContext(options.Options);
    var enumerable = context.Products.Include(p => p.Category).Where(p => p.Category.MainCat.Contains("shoes")).AsEnumerable();
    var t = enumerable.ToList();
    foreach (var e in enumerable)
      e.ToString();

    var query = context.Products.Include(p => p.Category).Where(p => p.Category.MainCat.Contains("shoes")).AsQueryable();
    var t2 = query.ToList();
    foreach (var q in query)
      q.ToString();

    var prods = await context.Products.Include(p => p.Category).Where(p => p.Category.MainCat.Contains("shoes")).ToListAsync();

    int g = 1;
  }


}//eo cls
