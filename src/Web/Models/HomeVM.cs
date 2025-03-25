using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Models;

namespace EStore.Web.Models;

public class HomeVM
{
  public int BasketCount {  get; set; }
  //public Basket? Basket { get; set; }
  public IEnumerable<ProductVM>? Products {  get; set; } 


}
