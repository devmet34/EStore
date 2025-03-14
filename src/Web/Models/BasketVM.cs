using EStore.Core.Entities.BasketAggregate;

namespace EStore.Web.Models;

public class BasketVM
{
  public IEnumerable<BasketItem>? BasketItems { get; set; }
  public decimal TotalPrice { get; set; } = 0;
  //public int Count { get; set; }

  public BasketVM(IEnumerable<BasketItem>? basketItems,decimal totalPrice)
  {
    BasketItems = basketItems;
    TotalPrice = totalPrice;
  }

  
}
