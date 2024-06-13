using Estore.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities
{
  public class Basket:BaseEntity
  {
    public string BuyerId {  get; private set; }
    public DateTime BasketCreatedAt { get; private set; }

    public ICollection<BasketItem> BasketItems { get; private set; }=new List<BasketItem>();

    
    public Basket(string buyerId )
    {
      buyerId.GuardNullOrEmpty();
      BuyerId = buyerId;
      BasketCreatedAt = DateTime.Now;
      
    }

    public void SetBasketItem(int productId, int qt, decimal price)
    {
      //qt.GuardNegative(); already guarded in basketitem
      qt.GuardZero();
      var basketItem = GetBasketItem(productId);
      if (basketItem != null)
      {
        if (basketItem.Qt == qt)
          throw new Exception("Same quantity already set");
        basketItem.SetQt(qt);
        return;
      }
      
      basketItem = new BasketItem(Id, productId, qt, price);
      BasketItems.Add(basketItem);


    }

    
    private void UpdateBasketItemQt(BasketItem basketItem,int qt)
    {

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

    public void IncrementQt(BasketItem basketItem)
    {
      //var basketItem = BasketItems.FirstOrDefault(bi => bi.ProductId == productId);
      basketItem?.SetQt(basketItem.Qt+1);
    }

    public void DecrementQt(BasketItem basketItem)
    {
      if (basketItem.Qt==1)
        BasketItems.Remove(basketItem);
      basketItem?.SetQt(basketItem.Qt - 1);
    }
    public void UpdateItemQt(int productId, int qt)
    {
      
      BasketItems.FirstOrDefault(bi => bi.ProductId == productId)?.SetQt(qt);

    }

    public void RemoveItem(int productId)
    {
      var basketItem=BasketItems.FirstOrDefault(bi => bi.ProductId == productId);

      BasketItems.Remove(basketItem ?? throw new Exception("Item to remove not found"));
    }

    public bool IsItemExist(int productId)
    {
      return BasketItems.Any(bi=>bi.ProductId == productId);

    }


  }
}
