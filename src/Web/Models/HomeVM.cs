using EStore.Core.Entities.BasketAggregate;

namespace EStore.Web.Models;

public class HomeVM
{
  public int BasketCount {  get; set; }
  //public Basket? Basket { get; set; }
  public IEnumerable<EStore.App.WebModels.ProductVM>? Products {  get; set; } 


}
