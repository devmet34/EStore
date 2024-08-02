using Estore.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities;

public class Product : BaseEntity
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
  /// Passing negative number will decrease total quantity, positive will increase it. 
  /// </summary>
  /// <param name="qt"></param>
  public void UpdateQt(int qt)
  {    
    Qt += qt;
  }

}//eq cls

