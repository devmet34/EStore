namespace EStore.Core.Models;

public class ProductVM
{
  public int Id { get; set; }
  public string? Name { get; set; }

  public decimal Price { get; set; }

  //public int Qt { get; set; }

  public string? PictureUri { get; set; }

  public ProductVM() { }

  public ProductVM(int id, string? name, decimal price, string? pictureUri)
  {
    Id = id;
    Name = name;
    Price = price;
    PictureUri = pictureUri;
  }
}
