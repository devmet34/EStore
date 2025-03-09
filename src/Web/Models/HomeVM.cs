using Estore.Core.Entities.BasketAggregate;

namespace EStore.Web.Models;

public class HomeVM
{
  public Basket? Basket { get; set; }
  public IEnumerable<Estore.App.WebModels.ProductVM>? Products {  get; set; } 


}
