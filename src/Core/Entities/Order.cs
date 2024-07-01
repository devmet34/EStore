using Estore.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities;
public class Order:BaseEntity
{
  public string BuyerId {  get; private set; }
  public DateTime CreatedAt { get; private set; }
  public decimal TotalPrice { get; private set; }
  public ICollection<OrderItem> OrderItems { get; set; }=new List<OrderItem>();

  public Order() { }
  
  public Order (Basket basket)
  {
    basket.BasketItems.GuardNull();
    BuyerId =basket.BuyerId;
    CreatedAt=DateTime.Now;
    TotalPrice = basket.TotalPrice;
    SetOrderItems (basket);

  }

  private void SetOrderItems(Basket basket)
  {
    foreach (var item in basket.BasketItems) 
    {
      decimal totalPrice = item.Price * item.Qt;
      OrderItems.Add(
        new OrderItem(Id,item.ProductId,item.Qt,totalPrice)
        );
      

    
    }
  }
   
}
