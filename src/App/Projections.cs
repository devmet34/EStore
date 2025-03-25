using EStore.Core.Entities;
using EStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EStore.App;
public class Projections
{
  public static Expression<Func<Product, ProductVM>> ProductToProductVM()
  {
    return product => new ProductVM() 
    { Id = product.Id, Name = product.Name, PictureUri = product.PictureUri, Price = product.Price };

  }
}
