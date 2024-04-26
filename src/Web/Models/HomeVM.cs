using Estore.Core.Entities;

namespace EStore.Web.Models;

public class HomeVM
{
  public Basket? Basket { get; set; }
  public IEnumerable<ProductVM>? Products {  get; set; } 


}
