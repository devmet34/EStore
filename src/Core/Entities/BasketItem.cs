using Estore.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities;
public class BasketItem:BaseEntity
{
  public int BasketId {  get; private set; }
  public int ProductId {  get; private set; }
  public Product? Product { get; set; }
  public int Qt {  get; private set; }
  public decimal Price {  get; private set; }
  
  //todo guards
  public BasketItem(int basketId,int productId, int qt,decimal price) 
  {    
    BasketId = basketId;
    ProductId = productId;
    SetQt(qt);
    Price = price;

  }

  public void SetQt(int qt)
  {
    Qt=qt.GuardNegative();
   
  }

}
