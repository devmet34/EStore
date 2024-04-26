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
  public static async Task Update(EStoreDbContext context)
  {
    await context.Products.ExecuteUpdateAsync
      (   x => x.SetProperty(p => p.Description, "test"));
  }
}
