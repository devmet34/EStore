using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Helpers;
public class UpdateProducts
{
  public static async Task UpdateProductsBatch(EStoreDbContext context)
  {
    int startIndex = 43;
    int range = 10;
    int it = 10;
    for(int iter=0;iter<5;iter++) 
    {
      await context.Products.Where(p => p.Id > startIndex & p.Id <= startIndex+range).ExecuteUpdateAsync
        (x =>
            x.SetProperty(p => p.Name, p => p.Name+it.ToString())
        );
      startIndex += range;
      it++;
    }
    return;

    int i = 1;
    int j = 10;
    int pageSize = 36;
    while (i < 4)
    {
      var prods = await context.Products.Take(pageSize).Skip(i * pageSize).ToListAsync();
      foreach (var prod in prods) {
        prod.UpdateName(prod.Name +' '+j);
        j++;
      }
      i++;
    }



    
  }
}
