using EStore.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Entities.BasketAggregate;
public class BasketItem : BaseEntity
{
  public int BasketId { get; private set; }
  public int ProductId { get; private set; }
  public Product? Product { get; set; }
  public int Qt { get; private set; }
  
 

  //todo guards
  public BasketItem(int basketId, int productId, int qt, Product? product=null)
  {
    BasketId = basketId;
    ProductId = productId;    
    SetQt(qt);
    Product = product ?? null;

  }

  private BasketItem(int basketId, int productId, int qt)
  {
    BasketId = basketId;
    ProductId = productId;
    
    Qt = qt;
  }

  public void SetQt(int qt)
  {
    Qt = qt.GuardNegative();

  }

  public void SetPrice(decimal price)
  {
    //Price = price.GuardZeroOrNegative();
  }


}
