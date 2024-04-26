using Estore.Core.Entities;
using Estore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infra.EF;
public class SpecEvaluator
{
  public static IQueryable<T> SetQuery<T>(IQueryable<T> query, ISpec<T> spec) where T : class
  {
    if (spec.WhereExp != null)
      query = query.Where(spec.WhereExp);

    if (spec.Includes?.Count > 0)
    {
      foreach (var include in spec.Includes)
      {
        query = query.Include(include);
      }
    }
    
    return query;
  }

  public static IQueryable<Basket> Query() 
  {
    string buyerId = "fefefd7e-d506-45ad-aa9d-7dc80cd15dc1";
    var q = Enumerable .Empty<Basket>().AsQueryable();
    q = q.Where(b => b.BuyerId == buyerId)
      .Include(b => b.BasketItems);
    return q;
    
  }
}
