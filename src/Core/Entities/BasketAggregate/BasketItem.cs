using EStore.Core.Extensions;

namespace EStore.Core.Entities.BasketAggregate;
public class BasketItem : BaseEntity
{
  public int BasketId { get; private set; }
  public int ProductId { get; private set; }
  public Product? Product { get; private set; }
  public decimal Price { get; private set; }
  public int Qt { get; private set; }

  //mc, Product param is needed for redis caching so product name, uri etc can be saved/loaded with basket.
  public BasketItem(int basketId, int productId, int qt, decimal price, Product? product = null)
  {
    BasketId = basketId;
    ProductId = productId;
    SetQt(qt);
    SetPrice(price);
    Product = product ?? null;
  }

  //mc, this constructor is required for EF bound.  
  private BasketItem(int basketId, int productId, int qt, decimal price)
  {
    BasketId = basketId;
    ProductId = productId;

    Qt = qt;
    Price = price;
  }


  public void SetQt(int qt)
  {
    Qt = qt.GuardNegative();

  }

  public void SetPrice(decimal price)
  {
    Price = price.GuardZeroOrNegative();
  }


}
