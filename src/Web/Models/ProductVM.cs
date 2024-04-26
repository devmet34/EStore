

namespace EStore.Web.Models;

public class ProductVM
{
  public int Id { get; set; }
  public string Name { get;  set; }
 
  public decimal Price { get;  set; }

  public int Qt {  get; set; }
 
  public string? PictureUri { get;  set; }
}
