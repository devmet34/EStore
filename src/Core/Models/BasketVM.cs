
using EStore.Core.Entities.BasketAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Models
{
  public class BasketVM
  {
    public IEnumerable<BasketItemVM>? BasketItems { get; init; }
    public decimal TotalPrice { get; init; } = 0;
    //public int Count { get; set; }

    public BasketVM(IEnumerable<BasketItemVM>? basketItems, decimal totalPrice)
    {
      BasketItems = basketItems;
      TotalPrice = totalPrice;
    }
  }
}
