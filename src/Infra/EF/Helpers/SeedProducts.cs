using Estore.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Helpers;
public static class SeedProducts
{
  
  public static async Task SeedProductsAsync(EStoreDbContext context)
  {
    int totalRecords = 100;
    context.GuardNull(nameof(context));
    if (!context.Database.CanConnect())
      throw new Exception("Error, cant connect to db");
    using var cont = context;
    

    

  }

}//eo class
