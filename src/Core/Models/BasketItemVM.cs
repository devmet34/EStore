
using EStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Models
{
  public class BasketItemVM
  {   
    public int BasketId { get; init; }
    public int ProductId { get; init; }
    public ProductVM? Product { get; init; }
    public int Qt { get; init; }

    public BasketItemVM(int basketId, int productId, ProductVM? product, int qt)
    {
      BasketId = basketId;
      ProductId = productId;
      Product = product;
      Qt = qt;
    }

    public BasketItemVM() { }

  }
}
