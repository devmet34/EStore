using EStore.Core.Extensions;
using EStore.Core.Interfaces;
using System.Text.Json.Serialization;

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

    /// <summary>
    /// mc, Set/add basketitem to basket. Product param is needed for redis caching so product name, uri etc can be saved/loaded with basket.
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="qt"></param>
    /// <param name="price"></param>
    /// <param name="product"></param>
    /// <exception cref="Exception"></exception>
    public void SetBasketItem(int productId, int qt, decimal price, Product? product = null)
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
      basketItem = product != null ? new BasketItem(Id, productId, qt, price, product) : new BasketItem(Id, productId, qt, price);

      TotalPrice += price * qt;
      BasketItems.Add(basketItem);

    }


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
      if (basketItem == null)
        throw new Exception("Item to remove not found");
      var itemPrice = basketItem!.Price;
      BasketItems.Remove(basketItem);
      TotalPrice -= itemPrice * basketItem.Qt;
    }

    public bool IsItemExist(int productId)
    {
      return BasketItems.Any(bi => bi.ProductId == productId);

    }

    public int BasketItemCount => BasketItems.Count;

  }
}
