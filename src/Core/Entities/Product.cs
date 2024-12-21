using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities;

public class Product : BaseEntity, IAggregateRoot
{

  public string Name { get; private set; }
  public string? Description { get; private set; }
  public Category? Category { get; private set; }
  public int? CategoryId { get; private set; }
  public Brand? Brand { get; private set; }
  public int? BrandId { get; private set; } 
  public decimal Price { get; private set; }
  public int Qt { get; private set; }
  public string? PictureUri { get; private set; }
  
  public byte[] Version { get; set; }   


  public Product(string name, int? categoryId, int? brandId, decimal price, int qt, string? description = null, string? pictureUri=null)
  {
    Name = name;
    Description = description;
    CategoryId = categoryId;
    BrandId = brandId;
    Price= price.GuardNegative();   
    Qt=qt.GuardNegative();    
    PictureUri = pictureUri;

  }

  public void UpdatePrice(decimal price)
  {
    Price= price.GuardZeroOrNegative();     
  }

  /// <summary>
  /// Passing negative number will decrease total quantity, positive will increase it. e.g:  UpdateQt(-2) will change qt 10 to 8.
  /// </summary>
  /// <param name="qt"></param>
  public void UpdateQt(int qt)
  {
    (Qt + qt).GuardNegative();
    Qt += qt;
  }

  public void UpdateName(string name) {
    name.GuardNullOrEmpty();
    Name = name;
  }

}//eq cls

