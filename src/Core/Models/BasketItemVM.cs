namespace EStore.Core.Models
{
  public class BasketItemVM
  {
    public int BasketId { get; init; }
    public int ProductId { get; init; }
    public ProductVM? Product { get; init; }
    public int Qt { get; init; }
    public decimal Price { get; init; }

    public BasketItemVM(int basketId, int productId, ProductVM? product, int qt, decimal price)
    {
      BasketId = basketId;
      ProductId = productId;
      Product = product;
      Qt = qt;
      Price = price;
    }

    public BasketItemVM() { }

  }
}
