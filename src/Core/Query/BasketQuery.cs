using Estore.Core.Entities;
using Estore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Query;
public class BasketQuery
{

  string buyerId; // "fefefd7e-d506-45ad-aa9d-7dc80cd15dc1";
  bool isIncludeItems;
  IQueryable<Basket> q;
  public BasketQuery(string buyerId, bool isIncludeItems=false)
  {
    
    this.buyerId = buyerId;
    this.isIncludeItems = isIncludeItems;
  }

  public IQueryable<Basket> GetQuery()
  {
    
    
    q = q.Where(b => b.BuyerId == this.buyerId)
      .Include(b => b.BasketItems);
    return q;
    
  }

}
