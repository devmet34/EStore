using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities.BasketAggregate
{
    public class Basket : BaseEntity, IAggregateRoot
    {
        public string BuyerId { get; private set; }
        public DateTime BasketCreatedAt { get; private set; }
        public decimal TotalPrice { get; private set; }

        public ICollection<BasketItem> BasketItems { get; private set; } = new List<BasketItem>();


        public Basket(string buyerId)
        {
            BuyerId = buyerId.GuardNullOrEmpty();
            BasketCreatedAt = DateTime.Now;

        }

        public void SetBasketItem(int productId, string productName, int qt, decimal price)
        {
            //qt.GuardNegative(); already guarded in basketitem
            qt.GuardZero();
            var basketItem = GetBasketItem(productId);
            if (basketItem != null)
            {
                if (basketItem.Qt == qt)
                    throw new Exception("Same quantity already set");

                TotalPrice += basketItem.Price * (qt - basketItem.Qt);
                basketItem.SetQt(qt);
                return;
            }

            basketItem = new BasketItem(Id, productId, productName, qt, price);
            TotalPrice += basketItem.Price * qt;
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
            TotalPrice -= basketItem.Price * basketItem.Qt;
        }

        public bool IsItemExist(int productId)
        {
            return BasketItems.Any(bi => bi.ProductId == productId);

        }


    }
}
