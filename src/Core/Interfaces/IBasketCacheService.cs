using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Interfaces;
public interface IBasketCacheService:IBasketService
{
  public Task SetBasketCountAsync(string buyerId,int count);
}
