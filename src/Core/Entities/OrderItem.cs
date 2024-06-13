using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities;
public class OrderItem:BaseEntity
{
  public int OrderId { get; private set; }
  public int ProductId { get; private set; }
  public Product Product { get; set; } //navigation property
  public int Qt { get; private set; }
  public decimal TotalPrice { get; private set; }

 
  public OrderItem() { }  //For Ef core
  public OrderItem(int orderId, int productId, int qt, decimal price)
  {
    OrderId = orderId;
    ProductId = productId;    
    Qt = qt;
    TotalPrice = price;
  }
}
