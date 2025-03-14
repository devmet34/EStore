using EStore.Core.Extensions;
using EStore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EStore.Core.Entities.BasketAggregate
{
  public class Basket : BaseEntity, IAggregateRoot
  {
    public string BuyerId { get; private set; }
    
    public DateTime BasketCreatedAt { get; init; }
    [JsonInclude]
    public decimal TotalPrice { get; private set; }
    [JsonInclude]
    public ICollection<BasketItem> BasketItems { get; private set; } = new List<BasketItem>();


    public Basket(string buyerId)
    {
      BuyerId = buyerId.GuardNullOrEmpty();
      BasketCreatedAt = DateTime.Now;

    }
    
    public void SetBasketItem(int productId, int qt, decimal price,Product? product=null)
    {
      //qt.GuardNegative(); already guarded in basketitem
      qt.GuardZero();
      var basketItem = GetBasketItem(productId);
      //mc basket item exist in basket
      if (basketItem != null)
      {
        if (basketItem.Qt == qt)
          throw new Exception("Same quantity already set");

        //mc deduct current item price from total for reset.
        TotalPrice += -basketItem.Qt * price;                

        basketItem.SetQt(qt);
        TotalPrice += price * qt;
        return;
      }
      basketItem = product != null ? new BasketItem(Id, productId, qt, product) : new BasketItem(Id, productId, qt);

      TotalPrice += price * qt;
      BasketItems.Add(basketItem);


    }


    /*
    public void AddItem( int productId,int qt=1 )
    {
      var basketItem = GetBasketItem(productId);
      if (basketItem!=null)
      {
        IncrementQt(basketItem);
        return;
      }
      basketItem = new BasketItem(Id, productId, qt);
      BasketItems.Add(basketItem);


    }
   */

    public BasketItem? GetBasketItem(int productId)
    {
      return BasketItems.FirstOrDefault(bi => bi.ProductId == productId);
    }

    public void UpdateItemQt(int productId, int qt)
    {

      BasketItems.FirstOrDefault(bi => bi.ProductId == productId)?.SetQt(qt);

    }

    public void RemoveBasketItem(int productId)
    {
      var basketItem = BasketItems.FirstOrDefault(bi => bi.ProductId == productId);

      BasketItems.Remove(basketItem ?? throw new Exception("Item to remove not found"));
      TotalPrice -= basketItem.Product!.Price * basketItem.Qt;
    }

    public bool IsItemExist(int productId)
    {
      return BasketItems.Any(bi => bi.ProductId == productId);

    }


  }
}
