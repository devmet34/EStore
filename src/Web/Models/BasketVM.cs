using Estore.Core.Entities;

namespace EStore.Web.Models;

public class BasketVM
{
  public IEnumerable<BasketItem>? BasketItems { get; set; }
  //public int Count { get; set; }

  
}
